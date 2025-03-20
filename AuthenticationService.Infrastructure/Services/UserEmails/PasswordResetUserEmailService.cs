using AuthenticationService.Domain.Models;
using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Infrastructure.Interfaces.Services;
using AuthenticationService.Shared.Dtos.Templates.Emails;
using AuthenticationService.Shared.Helpers;
using AuthenticationService.Shared.Interfaces.Services;
using AuthenticationService.Shared.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace AuthenticationService.Infrastructure.Services.UserEmails
{
    public class PasswordResetUserEmailService : IUserEmailService<PasswordResetEmailDto>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEmailQueueService _emailQueueService;

        public PasswordResetUserEmailService(IServiceScopeFactory serviceScopeFactory, IEmailQueueService emailQueueService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _emailQueueService = emailQueueService;
        }

        public void SendEmail(object applicationUserId)
        {
            // The current culture resets to the default after the request completes, losing the request-specific culture.  
            // To preserve it in a background thread, we capture it in an immutable, non-scoped object inside the closure.
            string culture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            Func<Task<Email>> buildEmailFuncAsync = async () =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                    string? emailTemplateBasePath = configuration["Paths:Templates:Emails:BasePath"];
                    string? emailTemplateFileName = configuration["Paths:Templates:Emails:Files:PasswordReset"];

                    if (string.IsNullOrWhiteSpace(emailTemplateBasePath) || string.IsNullOrWhiteSpace(emailTemplateFileName))
                    {
                        throw new InvalidOperationException("Email template path is not configured properly.");
                    }

                    string emailTemplateFullPath = Path.Combine(emailTemplateBasePath, emailTemplateFileName);

                    if (string.IsNullOrWhiteSpace(emailTemplateFileName))
                    {
                        throw new InvalidOperationException("Email template file name is not configured properly.");
                    }

                    var applicationUser = await userManager.FindByIdAsync((string)applicationUserId) ?? throw new KeyNotFoundException($"User with ID '{(string)applicationUserId}' not found.");
                    var emailDto = mapper.Map<PasswordResetEmailDto>(applicationUser);
                    var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(applicationUser);
                    emailDto.Token = WebUtility.UrlEncode(passwordResetToken);

                    var templateHtml = await HtmlRenderHelper.RenderHtmlFromFileTemplateAsync(emailTemplateFullPath, emailDto, culture);

                    Email email = new Email();
                    email.To.Add(applicationUser.Email);
                    email.Subject = EmailTemplateMessages.PasswordResetEmailSubject;
                    email.Body = templateHtml;

                    return email;
                }
            };

            _emailQueueService.EnqueueEmail(buildEmailFuncAsync);
        }

        public void SendNotification(object applicationUserId)
        {
            // TODO: Implement this method to send an email notification to the user through the Notification Service API
            throw new NotImplementedException();
        }
    }
}
