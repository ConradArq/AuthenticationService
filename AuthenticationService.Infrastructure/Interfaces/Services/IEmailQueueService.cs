using AuthenticationService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Interfaces.Services
{
    public interface IEmailQueueService
    {
        void EnqueueEmail(Func<Task<Email>> buildEmailFuncAsync);
        bool TryDequeue(out Func<Task<Email>> buildEmailFuncAsync);
    }
}
