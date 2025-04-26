using Polly.Retry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.PollyPolicies
{
    public class PollyPolicyRegistry : IPollyPolicyRegistry
    {
        public AsyncRetryPolicy RedisRetryPolicy => RedisPolicies.GetRedisRetryPolicy();
        public AsyncRetryPolicy SqlRetryPolicy => SqlPolicies.GetSqlRetryPolicy();
        public IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy => HttpPolicies.GetHttpRetryPolicy();
    }
}
