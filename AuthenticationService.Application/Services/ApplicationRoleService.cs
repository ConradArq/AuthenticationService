using AutoMapper;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Shared.Exceptions;
using AuthenticationService.Application.Features.ApplicationRole;
using AuthenticationService.Application.Features.ApplicationRole.Commands.Create;
using AuthenticationService.Application.Features.ApplicationRole.Commands.Update;
using AuthenticationService.Application.Features.ApplicationRole.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.ApplicationRole.Queries.Search;
using AuthenticationService.Application.Features.ApplicationRole.Queries.SearchPaginated;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Shared.Helpers;
using AuthenticationService.Shared.Resources;
using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Application.Features.ApplicationRole.Commands.AssignMenusToRole;
using AuthenticationService.Application.Features.ApplicationRole.Commands.RemoveMenusFromRole;
using System.Linq.Expressions;

namespace AuthenticationService.Application.Services
{
    public class ApplicationRoleService : IApplicationRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationRoleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<ApplicationRoleResponse>> CreateAsync(CreateApplicationRoleCommand request)
        {
            var roleRepository = _unitOfWork.ApplicationRoleRepository;

            bool roleExists = await roleRepository.RoleExistsAsync(request.Name);
            if (roleExists)
            {
                throw new ConflictException(ValidationMessages.ExistingRoleNameError);
            }

            var entity = _mapper.Map<ApplicationRole>(request);
            var (isSuccess, errors) = await roleRepository.CreateRoleAsync(entity);

            if (!isSuccess)
            {
                throw new Exception(string.Join(", ", errors));
            }

            await _unitOfWork.SaveAsync();
            return new ResponseDto<ApplicationRoleResponse>(_mapper.Map<ApplicationRoleResponse>(entity));
        }

        public async Task<ResponseDto<ApplicationRoleResponse>> UpdateAsync(string id, UpdateApplicationRoleCommand request)
        {
            var roleRepository = _unitOfWork.ApplicationRoleRepository;

            var entity = await roleRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            _mapper.Map(request, entity);
            var (isSuccess, errors) = await roleRepository.UpdateRoleAsync(entity);

            if (!isSuccess)
            {
                throw new Exception(string.Join(", ", errors));
            }

            await _unitOfWork.SaveAsync();
            return new ResponseDto<ApplicationRoleResponse>(_mapper.Map<ApplicationRoleResponse>(entity));
        }

        public async Task<ResponseDto<object>> DeleteAsync(string id)
        {
            var roleRepository = _unitOfWork.ApplicationRoleRepository;

            var entity = await roleRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            var (isSuccess, errors) = await roleRepository.DeleteRoleAsync(entity);
            if (!isSuccess)
            {
                throw new Exception(string.Join(", ", errors));
            }

            await _unitOfWork.SaveAsync();
            return new ResponseDto<object>();
        }

        public async Task<ResponseDto<ApplicationRoleMenuResponse>> AssignMenusToRoleAsync(AssignMenusToRoleCommand request)
        {
            // Check if ApplicationRole exists

            var applicationRole = await _unitOfWork.ApplicationRoleRepository.GetSingleAsync(request.ApplicationRoleId, 
                includesAndThenIncludes: new() { { x => x.ApplicationRoleMenus,
                        new List<Expression<Func<object, object>>>{ x=>((ApplicationRoleMenu)x).ApplicationMenu } }});

            if (applicationRole == null)
            {
                throw new NotFoundException((object)request.ApplicationRoleId);
            }

            // Check if all ApplicationMenuIds in the request exist

            var existingApplicationMenuIds = (await _unitOfWork.ApplicationMenuRepository.GetAsync(m => request.ApplicationMenuIds.Contains(m.Id))).Select(m => m.Id).ToList();

            var invalidApplicationMenuIds = request.ApplicationMenuIds.Except(existingApplicationMenuIds).ToList();
            if (invalidApplicationMenuIds.Any())
            {
                throw new NotFoundException(string.Format(GeneralMessages.NotFoundExceptionEntitiesMessage, string.Join(", ", invalidApplicationMenuIds)));
            }

            // Save new ApplicationRoleMenus in database

            foreach (var applicationMenuId in request.ApplicationMenuIds)
            {
                if (!applicationRole.ApplicationRoleMenus.Select(x => x.ApplicationMenuId).Contains(applicationMenuId))
                {
                    var applicationRoleMenu = _mapper.Map<ApplicationRoleMenu>(request);
                    applicationRoleMenu.ApplicationMenuId = applicationMenuId;
                    applicationRole.ApplicationRoleMenus.Add(applicationRoleMenu);
                    _unitOfWork.ApplicationRoleMenuRepository.Reload(applicationRoleMenu, x => x.ApplicationMenu); // Makes it available in the response
                }
            }
            await _unitOfWork.SaveAsync();

            // Response
            var applicationRoleMenuResponse = _mapper.Map<ApplicationRoleMenuResponse>(applicationRole);
            return new ResponseDto<ApplicationRoleMenuResponse>(_mapper.Map<ApplicationRoleMenuResponse>(applicationRoleMenuResponse));          
        }

        public async Task<ResponseDto<ApplicationRoleMenuResponse>> RemoveMenusFromRoleAsync(RemoveMenusFromRoleCommand request)
        {
            // Check if ApplicationRole exists

            var applicationRole = await _unitOfWork.ApplicationRoleRepository.GetSingleAsync(request.ApplicationRoleId,
                includesAndThenIncludes: new() { { x => x.ApplicationRoleMenus,
                        new List<Expression<Func<object, object>>>{ x=>((ApplicationRoleMenu)x).ApplicationMenu } }});

            if (applicationRole == null)
            {
                throw new NotFoundException((object)request.ApplicationRoleId);
            }

            // Check if all ApplicationMenuIds in the request exist

            var existingApplicationMenuIds = (await _unitOfWork.ApplicationMenuRepository.GetAsync(m => request.ApplicationMenuIds.Contains(m.Id))).Select(m => m.Id).ToList();

            var invalidApplicationMenuIds = request.ApplicationMenuIds.Except(existingApplicationMenuIds).ToList();
            if (invalidApplicationMenuIds.Any())
            {
                throw new NotFoundException(string.Format(GeneralMessages.NotFoundExceptionEntitiesMessage, string.Join(", ", invalidApplicationMenuIds)));
            }

            // Remove ApplicationRoleMenus from database

            foreach (var applicationMenuId in request.ApplicationMenuIds)
            {
                var applicationRoleMenu = applicationRole.ApplicationRoleMenus.FirstOrDefault(x => x.ApplicationMenuId == applicationMenuId);
                if(applicationRoleMenu != null)
                {
                    applicationRole.ApplicationRoleMenus.Remove(applicationRoleMenu);
                }          
            }
            await _unitOfWork.SaveAsync();

            // Response

            var applicationRoleMenuResponse = _mapper.Map<ApplicationRoleMenuResponse>(applicationRole);
            return new ResponseDto<ApplicationRoleMenuResponse>(_mapper.Map<ApplicationRoleMenuResponse>(applicationRoleMenuResponse));
        }

        public async Task<ResponseDto<ApplicationRoleResponse>> GetAsync(string id)
        {
            var entity = await _unitOfWork.ApplicationRoleRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            return new ResponseDto<ApplicationRoleResponse>(_mapper.Map<ApplicationRoleResponse>(entity));
        }

        public async Task<ResponseDto<IEnumerable<ApplicationRoleResponse>>> GetAllAsync()
        {
            var entities = await _unitOfWork.ApplicationRoleRepository.GetAsync();
            return new ResponseDto<IEnumerable<ApplicationRoleResponse>>(_mapper.Map<IEnumerable<ApplicationRoleResponse>>(entities));
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>> GetAllPaginatedAsync(GetAllPaginatedApplicationRoleQuery request)
        {
            var entities = await _unitOfWork.ApplicationRoleRepository.GetPaginatedAsync(request.PageNumber, request.PageSize);
            return new PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>(
                _mapper.Map<IEnumerable<ApplicationRoleResponse>>(entities.Data),
                request.PageNumber,
                request.PageSize,
                entities.TotalItems
            );
        }

        public async Task<ResponseDto<IEnumerable<ApplicationRoleResponse>>> SearchAsync(SearchApplicationRoleQuery request)
        {
            var searchExpression = QueryHelper.BuildPredicate<ApplicationRole>(request);
            var entities = await _unitOfWork.ApplicationRoleRepository.GetAsync(searchExpression);
            return new ResponseDto<IEnumerable<ApplicationRoleResponse>>(_mapper.Map<IEnumerable<ApplicationRoleResponse>>(entities));
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>> SearchPaginatedAsync(SearchPaginatedApplicationRoleQuery request)
        {
            var searchExpression = QueryHelper.BuildPredicate<ApplicationRole>(request);
            var entities = await _unitOfWork.ApplicationRoleRepository.GetPaginatedAsync(request.PageNumber, request.PageSize, searchExpression);
            return new PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>(
                _mapper.Map<IEnumerable<ApplicationRoleResponse>>(entities.Data),
                request.PageNumber,
                request.PageSize,
                entities.TotalItems
            );
        }
    }
}
