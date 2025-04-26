using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AuthenticationService.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Logging;
using AuthenticationService.Shared.Resources;

namespace AuthenticationService.Infrastructure.Services.BackgroundServices
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEmailQueueService _emailQueueService;

        public EmailBackgroundService(IServiceScopeFactory serviceScopeFactory, IEmailQueueService emailQueueService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _emailQueueService = emailQueueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<EmailBackgroundService>>();

                try
                {
                    var buildEmailFuncAsync = await _emailQueueService.DequeueEmailAsync(stoppingToken);
                    var email = await buildEmailFuncAsync();
                    await emailService.SendEmailAsync(email);

                    logger.LogInformation(string.Format(
                        GeneralMessages.EmailSentMessage,
                        email.Subject,
                        email.To,
                        email.CC == null ? GeneralMessages.NoneAvailableMessage.TrimEnd('.') : string.Join(", ", email.CC),
                        email.BCC == null ? GeneralMessages.NoneAvailableMessage.TrimEnd('.') : string.Join(", ", email.BCC)
                    ));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error in email background service.");
                }
            }
        }
    }
}