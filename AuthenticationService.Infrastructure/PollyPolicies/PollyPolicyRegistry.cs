using Polly.Retry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Infrastructure.Services.BackgroundServices;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Infrastructure.PollyPolicies
{
    public class PollyPolicyRegistry : IPollyPolicyRegistry
    {
        private readonly ILogger<PollyPolicyRegistry> _logger;

        public PollyPolicyRegistry(ILogger<PollyPolicyRegistry> logger) 
        {
            _logger = logger;
        }

        public AsyncRetryPolicy RedisRetryPolicy => RedisPolicies.GetRedisRetryPolicy(_logger);
        public AsyncRetryPolicy SqlRetryPolicy => SqlPolicies.GetSqlRetryPolicy(_logger);
        public IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy => HttpPolicies.GetHttpRetryPolicy(_logger);
        public IAsyncPolicy EmailPolicy => EmailPolicies.GetEmailPolicy(_logger);
    }
}
