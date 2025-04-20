using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthenticationService.Application.Behaviours;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Application.Mappings;
using AuthenticationService.Application.Services;
using System.Reflection;
using AuthenticationService.Application.Strategies.Delete;
using AuthenticationService.Application.Strategies;
using AuthenticationService.Application.Interfaces.Strategies.Delete;
using AuthenticationService.Application.Interfaces.Strategies.Delete.Factories;
using AuthenticationService.Application.Strategies.Delete.Factories;

namespace AuthenticationService.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationService, Services.AuthenticationService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
            services.AddScoped<IApplicationMenuService, ApplicationMenuService>();
            services.AddScoped<IStatusService, StatusService>();

            // FluentValidation scans the specified assembly for classes implementing AbstractValidator<T> using reflection and
            // automatically registers them in the DI container. `AddValidatorsFromAssemblyContaining<T>()` can also be used to
            // specify a type that resides within the assembly instead of explicitly referencing the assembly.
            services.AddFluentValidationClientsideAdapters().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Uncomment to enable integration between FluentValidation and ASP.NET Core's automatic model validation. Not needed because
            // validation is handled manually in ValidationBehavior. If enabled, to keep validation handling in the MediatR pipeline
            // via behaviors, set ApiBehaviorOptions.SuppressModelStateInvalidFilter to true to disable automatic 400 Bad Request responses.
            //// services.AddFluentValidationAutoValidation();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            // UnhandledExceptionBehaviour is registered early in the pipeline to catch all unhandled exceptions.
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            QueryProfile.InitializeMappings();

            services.AddSingleton(typeof(IDeletionStrategy<>), typeof(HardDeleteStrategy<>));
            services.AddSingleton(typeof(IDeletionStrategy<>), typeof(SoftDeleteStrategy<>));
            services.AddSingleton<IDeleteStrategyFactory, DeleteStrategyFactory>();

            return services;
        }
    }
}
