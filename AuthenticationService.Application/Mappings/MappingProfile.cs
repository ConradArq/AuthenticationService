using AutoMapper;
using Microsoft.AspNetCore.Http;
using AuthenticationService.Application.Features.ApplicationUser;
using AuthenticationService.Application.Features.ApplicationUser.Commands.Update;
using AuthenticationService.Application.Features.ApplicationRole;
using AuthenticationService.Application.Features.ApplicationRole.Commands.Create;
using AuthenticationService.Application.Features.ApplicationRole.Commands.Update;
using AuthenticationService.Application.Features.ApplicationMenu;
using AuthenticationService.Application.Features.ApplicationMenu.Commands.Create;
using AuthenticationService.Application.Features.ApplicationMenu.Commands.Update;
using AuthenticationService.Application.Features.Status;
using AuthenticationService.Application.Features.Status.Commands.Create;
using AuthenticationService.Application.Features.Status.Commands.Update;
using AuthenticationService.Domain.Models;
using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Shared.Dtos.ApplicationUser;
using AuthenticationService.Application.Features.ApplicationRole.Commands.AssignMenusToRole;
using AuthenticationService.Application.Features.ApplicationRole.Commands.RemoveMenusFromRole;
using AuthenticationService.Application.Features.Identity.Commands.Register;
using AuthenticationService.Application.Features.Authentication.Commands.GoogleLogin;
using AuthenticationService.Application.Features.Authentication.Commands.MicrosoftLogin;

namespace AuthenticationService.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // When using ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)) in mappings, if src is a
            // nullable value type and dest isn't nullable, and src is passed as null, AutoMapper will convert it  to its corresponding
            // default value (e.g., 0 for int, false for bool) before mapping. This behavior bypasses the condition, causing unintended
            // overwrites of destination values. To prevent this, we explicitly map nullable value types to their non-nullable
            // counterparts using ConvertUsing, ensuring that dest is left unaltered when src is null. 
            // For reference types, the condition works as expected, so no additional configuration is needed.
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<long?, long>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<short?, short>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<byte?, byte>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<float?, float>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<double?, double>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<char?, char>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<Guid?, Guid>().ConvertUsing((src, dest) => src ?? dest);

            // Authentication OAuth Flow
            CreateMap<GoogleLoginCommand, RegisterCommand>();
            CreateMap<MicrosoftLoginCommand, RegisterCommand>();
            CreateMap<RegisterCommand, UpdateApplicationUserCommand>();

            // Identity
            CreateMap<RegisterCommand, ApplicationUser>();

            // ApplicationUser
            CreateMap<UpdateApplicationUserCommand, ApplicationUser?>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ApplicationUser, ApplicationUserResponse>()
                .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => src.ApplicationUserRoles.Select(x => x.ApplicationRole.Name)));
            // Used in the identity workflow
            CreateMap<ApplicationUserResponseDto, ApplicationUserResponse>();
            CreateMap<ResponseDto<ApplicationUserResponseDto>, ResponseDto<ApplicationUserResponse>>();

            // ApplicationRole
            CreateMap<CreateApplicationRoleCommand, ApplicationRole>();
            CreateMap<UpdateApplicationRoleCommand, ApplicationRole?>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ApplicationRole?, ApplicationRoleResponse>();

            // ApplicationRoleMenu
            CreateMap<AssignMenusToRoleCommand, ApplicationRoleMenu>();
            CreateMap<RemoveMenusFromRoleCommand, ApplicationRoleMenu>();
            CreateMap<ApplicationRole, ApplicationRoleMenuResponse>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Menus, opt => opt.MapFrom(src => src.ApplicationRoleMenus.Select(x=>x.ApplicationMenu)));

            // ApplicationMenu
            CreateMap<CreateApplicationMenuCommand, ApplicationMenu>();
            CreateMap<UpdateApplicationMenuCommand, ApplicationMenu?>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ApplicationMenu?, ApplicationMenuResponse>();

            // Status
            CreateMap<CreateStatusCommand, Status>();
            CreateMap<UpdateStatusCommand, Status?>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Status?, ResponseStatus>();

            // FileAttachment
            CreateMap<IFormFile, FileAttachment>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => ConvertToByteArray(src)));
        }

        // This method does not work as expected - When the source property is a nullable value type and the destination property is a
        // non-nullable value type, the source property is boxed as its default value in the parameter sourceMember, regardless of
        // whether it is null. This behavior bypasses null checks and results in unintended mappings. 
        // To address this, we need explicit mappings for all nullable value types when mapped to non-nullable types, as demonstrated earlier.
        //private static bool IgnoreIfNull<TSource, TDestination>(TSource source, TDestination destination, object sourceMember)
        //{
        //    // If the sourceMember is null, skip mapping
        //    if (sourceMember == null)
        //        return false;

        //    Type sourceType = sourceMember.GetType();

        //    // Check if the sourceMember is a numeric type and skip mapping if <= 0
        //    if (sourceMember is int intValue && intValue <= 0)
        //        return false;
        //    if (sourceMember is long longValue && longValue <= 0)
        //        return false;
        //    if (sourceMember is double doubleValue && doubleValue <= 0)
        //        return false;
        //    if (sourceMember is decimal decimalValue && decimalValue <= 0)
        //        return false;
        //    if (sourceMember is float floatValue && floatValue <= 0)
        //        return false;

        //    // Nullable value types (e.g., int?, DateTime?) are structs (Nullable<>), so they are never actually null.
        //    // Instead, they have a HasValue property, which is false if they hold no value.
        //    // The null check above won't catch this, so we check HasValue to skip mapping when it's false.
        //    if (Nullable.GetUnderlyingType(sourceType) != null) // Checks if the type is Nullable<>
        //    {
        //        var hasValue = sourceType.GetProperty("HasValue")?.GetValue(sourceMember) as bool?;
        //        if (hasValue == false)
        //            return false;
        //    }

        //    return true;
        //}

        private static byte[] ConvertToByteArray(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
