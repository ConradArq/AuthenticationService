using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace AuthenticationService.Infrastructure.PollyPolicies
{
    public static class RedisPolicies
    {
        public static AsyncRetryPolicy GetRedisRetryPolicy(ILogger<PollyPolicyRegistry> logger)
        {
            return Policy
                .Handle<RedisConnectionException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * attempt),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        logger.LogError($"[Redis] Retry {retryCount} after {timeSpan.TotalMilliseconds}ms due to {exception.GetType().Name}");
                    });
        }
    }
}
