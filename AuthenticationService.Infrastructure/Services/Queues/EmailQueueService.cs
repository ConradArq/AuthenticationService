using AuthenticationService.Domain.Models;
using AuthenticationService.Infrastructure.Interfaces.Services;
using System.Collections.Concurrent;

namespace AuthenticationService.Infrastructure.Services.Queues
{
    public class EmailQueueService : IEmailQueueService
    {
        private readonly ConcurrentQueue<Func<Task<Email>>> _emailQueue = new ConcurrentQueue<Func<Task<Email>>>();

        public void EnqueueEmail(Func<Task<Email>> buildEmailFuncAsync)
        {
            _emailQueue.Enqueue(buildEmailFuncAsync);
        }

        public bool TryDequeue(out Func<Task<Email>> buildEmailFuncAsync)
        {
            return _emailQueue.TryDequeue(out buildEmailFuncAsync!);
        }
    }
}
