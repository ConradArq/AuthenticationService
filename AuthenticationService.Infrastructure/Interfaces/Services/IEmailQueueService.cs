using AuthenticationService.Domain.Models;

namespace AuthenticationService.Infrastructure.Interfaces.Services
{
    public interface IEmailQueueService
    {
        void EnqueueEmail(Func<Task<Email>> buildEmailFuncAsync);
        Task<bool> EnqueueEmailAsync(Func<Task<Email>> emailBuilder, Dictionary<string, string> distributedKeyParts);
        Task<Func<Task<Email>>> DequeueEmailAsync(CancellationToken cancellationToken);
    }
}