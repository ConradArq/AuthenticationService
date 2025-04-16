using AutoMapper;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Shared.Exceptions;
using AuthenticationService.Application.Features.ApplicationMenu;
using AuthenticationService.Application.Features.ApplicationMenu.Commands.Create;
using AuthenticationService.Application.Features.ApplicationMenu.Commands.Update;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.Search;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.SearchPaginated;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Domain.Models.Entities;
using System.Linq.Expressions;
using AuthenticationService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Application.Services
{
    internal class ApplicationMenuService : IApplicationMenuService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationMenuService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<ApplicationMenuResponse>> CreateAsync(CreateApplicationMenuCommand request)
        {
            var entity = _mapper.Map<ApplicationMenu>(request);

            _unitOfWork.ApplicationMenuRepository.Create(entity);
            await _unitOfWork.SaveAsync();

            var response = new ResponseDto<ApplicationMenuResponse>(_mapper.Map<ApplicationMenuResponse>(entity));
            return response;
        }

        public async Task<ResponseDto<ApplicationMenuResponse>> UpdateAsync(int id, UpdateApplicationMenuCommand request)
        {
            var entity = await _unitOfWork.ApplicationMenuRepository.GetSingleAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            _mapper.Map(request, entity);
            await _unitOfWork.SaveAsync();

            var response = new ResponseDto<ApplicationMenuResponse>(_mapper.Map<ApplicationMenuResponse>(entity));
            return response;
        }

        public async Task<ResponseDto<object>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.ApplicationMenuRepository.GetSingleAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            _unitOfWork.ApplicationMenuRepository.Delete(entity);
            await _unitOfWork.SaveAsync();

            var response = new ResponseDto<object>();
            return response;
        }

        public async Task<ResponseDto<ApplicationMenuResponse>> GetAsync(int id)
        {
            var entity = await _unitOfWork.ApplicationMenuRepository.GetSingleAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(id);
            }

            var entities = await _unitOfWork.ApplicationMenuRepository.GetAsync();
            ApplicationMenuResponse responseApplicationMenu = MapMenu(entities, entity);

            var response = new ResponseDto<ApplicationMenuResponse>(responseApplicationMenu);
            return response;
        }

        public async Task<ResponseDto<IEnumerable<ApplicationMenuResponse>>> GetAllAsync(RequestDto? requestDto)
        {
            var entities = await _unitOfWork.ApplicationMenuRepository.GetAsync(
                orderBy: BuildOrderByFunction<ApplicationMenu>(requestDto),
                includes: x => x.Include(x => x.ApplicationRoleMenus)
            );

            var parentMenus = entities.Where(x => x.ParentApplicationMenuId == null).OrderBy(x => x.Order).ToList();

            List<ApplicationMenuResponse> responseApplicationMenus = new List<ApplicationMenuResponse>();

            foreach (var parentMenu in parentMenus)
            {
                responseApplicationMenus.Add(MapMenu(entities, parentMenu));
            }

            var response = new ResponseDto<IEnumerable<ApplicationMenuResponse>>(responseApplicationMenus);
            return response;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>> GetAllPaginatedAsync(GetAllPaginatedApplicationMenuQuery request)
        {
            var entities = await _unitOfWork.ApplicationMenuRepository.GetPaginatedAsync(
                request.PageNumber,
                request.PageSize,
                orderBy: BuildOrderByFunction<ApplicationMenu>(request),
                includes: x => x.Include(x => x.ApplicationRoleMenus)
            );

            List<ApplicationMenuResponse> responseApplicationMenus = new List<ApplicationMenuResponse>();

            var parentMenus = entities.Data.Where(x => x.ParentApplicationMenuId == null).OrderBy(x => x.Order).ToList();

            foreach (var parentMenu in parentMenus)
            {
                responseApplicationMenus.Add(MapMenu(entities.Data, parentMenu));
            }

            var response = new PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>(responseApplicationMenus, request.PageNumber, request.PageSize, entities.TotalItems);
            return response;
        }

        public async Task<ResponseDto<IEnumerable<ApplicationMenuResponse>>> SearchAsync(SearchApplicationMenuQuery request)
        {
            List<ApplicationMenuResponse> responseApplicationMenus = new List<ApplicationMenuResponse>();
            Expression<Func<ApplicationMenu, bool>>? additionalCondition = null;

            if (request.roleId != null)
            {
                additionalCondition = x => x.ApplicationRoleMenus != null && x.ApplicationRoleMenus.Any(x => x.ApplicationRoleId == request.roleId);
            }

            var searchExpression = BuildPredicate(request, additionalCondition: additionalCondition);

            var entities = await _unitOfWork.ApplicationMenuRepository.GetAsync(
                searchExpression,
                orderBy: BuildOrderByFunction<ApplicationMenu>(request),
                includes: x => x.Include(x => x.ApplicationRoleMenus)
            );

            var parentMenus = entities.Where(x => x.ParentApplicationMenuId == null).OrderBy(x => x.Order).ToList();

            foreach (var parentMenu in parentMenus)
            {
                responseApplicationMenus.Add(MapMenu(entities, parentMenu));
            }

            var response = new ResponseDto<IEnumerable<ApplicationMenuResponse>>(responseApplicationMenus);
            return response;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>> SearchPaginatedAsync(SearchPaginatedApplicationMenuQuery request)
        {
            List<ApplicationMenuResponse> responseApplicationMenus = new List<ApplicationMenuResponse>();
            Expression<Func<ApplicationMenu, bool>>? additionalCondition = null;

            if (request.roleId != null)
            {
                additionalCondition = x => x.ApplicationRoleMenus != null && x.ApplicationRoleMenus.Any(x => x.ApplicationRoleId == request.roleId);
            }

            var searchExpression = BuildPredicate(request, additionalCondition: additionalCondition);

            var entities = await _unitOfWork.ApplicationMenuRepository.GetPaginatedAsync(
                request.PageNumber,
                request.PageSize,
                searchExpression,
                orderBy: BuildOrderByFunction<ApplicationMenu>(request),
                includes: x => x.Include(x => x.ApplicationRoleMenus)
            );

            var parentMenus = entities.Data.Where(x => x.ParentApplicationMenuId == null).OrderBy(x => x.Order).ToList();

            foreach (var parentMenu in parentMenus)
            {
                responseApplicationMenus.Add(MapMenu(entities.Data, parentMenu));
            }

            var response = new PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>(responseApplicationMenus, request.PageNumber, request.PageSize, entities.TotalItems);
            return response;
        }

        private ApplicationMenuResponse MapMenu(IReadOnlyList<ApplicationMenu> menus, ApplicationMenu parentMenu)
        {
            var responseParentMenu = _mapper.Map<ApplicationMenuResponse>(parentMenu);

            var subMenus = menus.Where(x => x.ParentApplicationMenuId == parentMenu.Id).OrderBy(x => x.Order).ToList();
            foreach (var subMenu in subMenus)
            {
                responseParentMenu.SubMenus.Add(MapMenu(menus, subMenu));
            }

            return responseParentMenu;
        }
    }
}