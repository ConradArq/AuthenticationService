using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace AuthenticationService.Infrastructure.PollyPolicies
{
    public static class RedisPolicies
    {
        public static AsyncRetryPolicy GetRedisRetryPolicy()
        {
            return Policy
                .Handle<RedisConnectionException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * attempt),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        // TODO: Implement custom logging
                        Console.WriteLine($"[Redis] Retry {retryCount} after {timeSpan.TotalMilliseconds}ms due to {exception.GetType().Name}");
                    });
        }
    }
}
