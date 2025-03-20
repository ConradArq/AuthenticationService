using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Interfaces.Services
{
    public interface ILogQueueService
    {
        void EnqueueLog(object logEntry);
        Task<object?> DequeueLogAsync(CancellationToken cancellationToken);
    }
}
