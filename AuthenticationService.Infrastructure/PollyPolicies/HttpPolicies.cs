using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.PollyPolicies
{
    public static class HttpPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() // Handles 5xx and 408 timeout
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests) // Handle 429 rate limiting
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // exponential backoff: 2s, 4s, 8s...
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        // TODO: Implement custom logging
                        Console.WriteLine($"[HTTP] Retry {retryAttempt} after {timespan.TotalSeconds}s due to {outcome.Exception?.GetType().Name ?? outcome.Result.StatusCode.ToString()}");
                    });
        }
    }
}
