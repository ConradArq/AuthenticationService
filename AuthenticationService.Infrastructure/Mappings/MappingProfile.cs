using AutoMapper;
using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Shared.Dtos.ApplicationUser;
using AuthenticationService.Shared.Dtos.Templates.Emails;
using AuthenticationService.Infrastructure.Configuration;
using AuthenticationService.Shared.Dtos.Authentication;

namespace AuthenticationService.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Identity workflow
            CreateMap<ApplicationUser, ApplicationUserResponseDto>();
            CreateMap<ApplicationUser, EmailConfirmationEmailDto>();          
            CreateMap<ApplicationUser, PasswordResetEmailDto>();          
            CreateMap<ApplicationUser, WelcomeEmailDto>();
            CreateMap<ApplicationUser, TwoFactorOtpEmailDto>();

            CreateMap<JwtSettings, JwtSettingsDto>();
        }
    }
}
