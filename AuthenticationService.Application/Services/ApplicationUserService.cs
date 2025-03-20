using AutoMapper;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Shared.Exceptions;
using AuthenticationService.Application.Features.ApplicationUser;
using AuthenticationService.Application.Features.ApplicationUser.Commands.Update;
using AuthenticationService.Application.Features.ApplicationUser.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.ApplicationUser.Queries.Search;
using AuthenticationService.Application.Features.ApplicationUser.Queries.SearchPaginated;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Shared.Helpers;
using AuthenticationService.Shared.Resources;
using AuthenticationService.Domain.Interfaces.Repositories;

namespace AuthenticationService.Application.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationUserService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<ApplicationUserResponse>> UpdateAsync(string id, UpdateApplicationUserCommand request)
        {
            var userRepository = _unitOfWork.ApplicationUserRepository;
            var roleRepository = _unitOfWork.ApplicationRoleRepository;

            var entity = await userRepository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new NotFoundException($"User with ID {id} not found.");
            }

            if (request.UserName != null && request.UserName != entity.UserName)
            {
                var existingUserByUserName = await userRepository.GetByUserNameAsync(request.UserName);
                if (existingUserByUserName != null)
                {
                    throw new ConflictException(string.Format(AuthMessages.UserWithUserNameAlreadyExistsMessage, request.UserName));
                }
            }

            _mapper.Map(request, entity);

            await _unitOfWork.CompleteTransactionAsync(async () =>
            {
                var (isSuccess, errors) = await userRepository.UpdateUserAsync(entity);
                if (!isSuccess)
                {
                    throw new Exception(string.Join(" ", errors));
                }

                // Handle user's roles
                if (request.RoleNames != null)
                {
                    foreach (var roleName in request.RoleNames)
                    {
                        if (!await roleRepository.RoleExistsAsync(roleName))
                        {
                            throw new NotFoundException(string.Format(AuthMessages.RoleNotFoundMessage, roleName));
                        }
                    }

                    var currentRolesNames = await userRepository.GetRolesAsync(entity);
                    var rolesToRemove = currentRolesNames.Except(request.RoleNames).ToList();
                    var rolesToAdd = request.RoleNames.Except(currentRolesNames).ToList();

                    if (rolesToRemove.Any())
                    {
                        var removeRolesResult = await userRepository.RemoveRolesAsync(entity, rolesToRemove);
                        if (!removeRolesResult.IsSuccess)
                        {
                            throw new Exception(string.Join(" ", removeRolesResult.Errors));
                        }
                    }

                    if (rolesToAdd.Any())
                    {
                        var addRolesResult = await userRepository.AssignRolesAsync(entity, rolesToAdd);
                        if (!addRolesResult.IsSuccess)
                        {
                            throw new Exception(string.Join(" ", addRolesResult.Errors));
                        }
                    }
                }
            });

            var applicationUserResponse = _mapper.Map<ApplicationUserResponse>(entity);

            applicationUserResponse.RoleNames = (await roleRepository
                .GetAsync(role => role.ApplicationUserRoles.Any(userRole => userRole.UserId == entity.Id)))
                .Select(role => role.Name!)
                .ToList();

            return new ResponseDto<ApplicationUserResponse>(applicationUserResponse);
        }

        public async Task<ResponseDto<object>> DeleteAsync(string id)
        {
            var userRepository = _unitOfWork.ApplicationUserRepository;
            var entity = await userRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            var (isSuccess, errors) = await userRepository.DeleteUserAsync(entity);
            if (!isSuccess)
            {
                throw new Exception(string.Join(" ", errors));
            }

            await _unitOfWork.SaveAsync();
            return new ResponseDto<object>();
        }

        public async Task<ResponseDto<ApplicationUserResponse>> GetAsync(string id)
        {
            var entity = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            return new ResponseDto<ApplicationUserResponse>(_mapper.Map<ApplicationUserResponse>(entity));
        }

        public async Task<ResponseDto<IEnumerable<ApplicationUserResponse>>> GetAllAsync()
        {
            var entities = await _unitOfWork.ApplicationUserRepository.GetAsync();
            return new ResponseDto<IEnumerable<ApplicationUserResponse>>(_mapper.Map<IEnumerable<ApplicationUserResponse>>(entities));
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>> GetAllPaginatedAsync(GetAllPaginatedApplicationUserQuery request)
        {
            var entities = await _unitOfWork.ApplicationUserRepository.GetPaginatedAsync(request.PageNumber, request.PageSize);
            return new PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>(
                _mapper.Map<IEnumerable<ApplicationUserResponse>>(entities.Data),
                request.PageNumber,
                request.PageSize,
                entities.TotalItems
            );
        }

        public async Task<ResponseDto<IEnumerable<ApplicationUserResponse>>> SearchAsync(SearchApplicationUserQuery request)
        {
            var searchExpression = QueryHelper.BuildPredicate<ApplicationUser>(request);
            var entities = await _unitOfWork.ApplicationUserRepository.GetAsync(searchExpression);
            return new ResponseDto<IEnumerable<ApplicationUserResponse>>(_mapper.Map<IEnumerable<ApplicationUserResponse>>(entities));
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>> SearchPaginatedAsync(SearchPaginatedApplicationUserQuery request)
        {
            var searchExpression = QueryHelper.BuildPredicate<ApplicationUser>(request);
            var entities = await _unitOfWork.ApplicationUserRepository.GetPaginatedAsync(request.PageNumber, request.PageSize, searchExpression);
            return new PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>(
                _mapper.Map<IEnumerable<ApplicationUserResponse>>(entities.Data),
                request.PageNumber,
                request.PageSize,
                entities.TotalItems
            );
        }
    }
}