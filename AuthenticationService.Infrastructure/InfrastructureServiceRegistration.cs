using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthenticationService.Infrastructure.Configuration;
using AuthenticationService.Infrastructure.Interfaces.Services;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.Repositories;
using AuthenticationService.Infrastructure.Services;
using AuthenticationService.Infrastructure.Services.Queues;
using AuthenticationService.Infrastructure.Services.BackgroundServices;
using AuthenticationService.Infrastructure.Providers;
using AuthenticationService.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Shared.Interfaces.Services;
using AuthenticationService.Shared.Dtos.Templates.Emails;
using AuthenticationService.Infrastructure.Factories;
using AuthenticationService.Infrastructure.Services.UserEmails;
using AuthenticationService.Shared.Interfaces.Providers;
using AuthenticationService.Shared.Interfaces.Factories;

namespace AuthenticationService.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthenticationServiceDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
                options.EnableSensitiveDataLogging();
                ////*,sqlServerOptions => sqlServerOptions.CommandTimeout(60)*/)
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            // Configures UserManager and RoleManager to use AuthenticationServiceDbContext
            .AddEntityFrameworkStores<AuthenticationServiceDbContext>()
            .AddDefaultTokenProviders();

            var redisConfig = configuration.GetSection("Redis");
            var redisHost = redisConfig["Host"] ?? throw new ArgumentNullException("Redis Host is not configured.");
            var redisPort = redisConfig["Port"] ?? throw new ArgumentNullException("Redis Port is not configured.");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{redisHost}:{redisPort}";
                options.InstanceName = redisConfig["InstanceName"];
            });

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddHttpClient();

            services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IApplicationMenuRepository, ApplicationMenuRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IApplicationRoleRepository, ApplicationRoleRepository>();
            services.AddScoped<IApplicationRoleMenuRepository, ApplicationRoleMenuRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddSingleton<IEmailNotificationServiceFactory, EmailNotificationServiceFactory>();
            services.AddScoped<IUserEmailService<EmailConfirmationEmailDto>, EmailConfirmationUserEmailService>();
            services.AddScoped<IUserEmailService<PasswordResetEmailDto>, PasswordResetUserEmailService>();
            services.AddScoped<IUserEmailService<WelcomeEmailDto>, WelcomeUserEmailService>();
            services.AddScoped<IUserEmailService<TwoFactorOtpEmailDto>, TwoFactorOtpUserEmailService>();

            services.AddSingleton<IEmailQueueService, EmailQueueService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddSingleton<IHttpService, HttpService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            services.AddHostedService<EmailBackgroundService>();

            return services;
        }
    }
}
