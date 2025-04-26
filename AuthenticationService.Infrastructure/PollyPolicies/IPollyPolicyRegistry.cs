using Polly.Retry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.PollyPolicies
{
    public interface IPollyPolicyRegistry
    {
        AsyncRetryPolicy RedisRetryPolicy { get; }
        AsyncRetryPolicy SqlRetryPolicy { get; }
        IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy { get; }
    }
}
