using AutoMapper;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Shared.Exceptions;
using AuthenticationService.Application.Features.Status;
using AuthenticationService.Application.Features.Status.Commands.Create;
using AuthenticationService.Application.Features.Status.Commands.Update;
using AuthenticationService.Application.Features.Status.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.Status.Queries.Search;
using AuthenticationService.Application.Features.Status.Queries.SearchPaginated;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Shared.Helpers;
using AutoMapper.QueryableExtensions;

namespace AuthenticationService.Application.Services
{
    internal class StatusService : IStatusService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public StatusService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<ResponseStatus>> CreateAsync(CreateStatusCommand request)
        {
            var entity = _mapper.Map<Status>(request);

            _unitOfWork.StatusRepository.Create(entity);
            await _unitOfWork.SaveAsync();

            var response = new ResponseDto<ResponseStatus>(_mapper.Map<ResponseStatus>(entity));
            return response;
        }

        public async Task<ResponseDto<ResponseStatus>> UpdateAsync(int id, UpdateStatusCommand request)
        {
            var entity = await _unitOfWork.StatusRepository.GetSingleAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            _mapper.Map(request, entity);
            await _unitOfWork.SaveAsync();

            var response = new ResponseDto<ResponseStatus>(_mapper.Map<ResponseStatus>(entity));
            return response;
        }

        public async Task<ResponseDto<object>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.StatusRepository.GetSingleAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            _unitOfWork.StatusRepository.Delete(entity);
            await _unitOfWork.SaveAsync();

            var response = new ResponseDto<object>();
            return response;
        }

        public async Task<ResponseDto<ResponseStatus>> GetAsync(int id)
        {
            var entity = await _unitOfWork.StatusRepository.GetSingleAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            var response = new ResponseDto<ResponseStatus>(_mapper.Map<ResponseStatus>(entity));
            return response;
        }

        public async Task<ResponseDto<IEnumerable<ResponseStatus>>> GetAllAsync(RequestDto? requestDto)
        {
            var selector = new Func<IQueryable<Status>, IQueryable<ResponseStatus>>(query => query
                 .ProjectTo<ResponseStatus>(_mapper.ConfigurationProvider)
             );

            var responseDtos = await _unitOfWork.StatusRepository.GetAsync(
                orderBy: QueryHelper.BuildOrderByFunction<Status>(requestDto),
                selector: selector
            );

            var response = new ResponseDto<IEnumerable<ResponseStatus>>(responseDtos);
            return response;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ResponseStatus>>> GetAllPaginatedAsync(GetAllPaginatedStatusQuery request)
        {
            var entities = await _unitOfWork.StatusRepository.GetPaginatedAsync(request.PageNumber, request.PageSize, orderBy: BuildOrderByFunction<Status>(request));

            var response = new PaginatedResponseDto<IEnumerable<ResponseStatus>>(_mapper.Map<IEnumerable<ResponseStatus>>(entities.Data), request.PageNumber, request.PageSize, entities.TotalItems);
            return response;
        }

        public async Task<ResponseDto<IEnumerable<ResponseStatus>>> SearchAsync(SearchStatusQuery request)
        {
            var searchExpression = BuildPredicate<Status>(request);
            var entities = await _unitOfWork.StatusRepository.GetAsync(searchExpression, orderBy: BuildOrderByFunction<Status>(request));

            var response = new ResponseDto<IEnumerable<ResponseStatus>>(_mapper.Map<IEnumerable<ResponseStatus>>(entities));
            return response;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ResponseStatus>>> SearchPaginatedAsync(SearchPaginatedStatusQuery request)
        {
            var searchExpression = BuildPredicate<Status>(request);
            var entities = await _unitOfWork.StatusRepository.GetPaginatedAsync(request.PageNumber, request.PageSize, searchExpression, orderBy: BuildOrderByFunction<Status>(request));

            var response = new PaginatedResponseDto<IEnumerable<ResponseStatus>>(_mapper.Map<IEnumerable<ResponseStatus>>(entities.Data), request.PageNumber, request.PageSize, entities.TotalItems);
            return response;
        }
    }
}
