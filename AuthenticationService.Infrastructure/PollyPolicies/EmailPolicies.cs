using Microsoft.Extensions.Logging;
using Polly.Timeout;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.PollyPolicies
{
    public static class EmailPolicies
    {
        public static IAsyncPolicy GetEmailPolicy(ILogger logger)
        {
            var retryPolicy = Policy
                .Handle<SmtpException>()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(300 * attempt),
                    onRetry: (exception, timespan, retryCount, context) =>
                    {
                        logger.LogWarning(exception, "[Email] Retry {RetryCount} after {Delay}ms", retryCount, timespan.TotalMilliseconds);
                    });

            var timeoutPolicy = Policy
                .TimeoutAsync(
                    TimeSpan.FromSeconds(10),
                    TimeoutStrategy.Pessimistic);

            var circuitBreakerPolicy = Policy
                .Handle<SmtpException>()
                .Or<TimeoutRejectedException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (exception, breakDelay) =>
                    {
                        logger.LogWarning(exception, "[Email] Circuit broken for {BreakDelay}ms", breakDelay.TotalMilliseconds);
                    },
                    onReset: () => logger.LogInformation("[Email] Circuit reset."),
                    onHalfOpen: () => logger.LogInformation("[Email] Circuit is half-open, next call is a trial.")
                );

            return Policy.WrapAsync(retryPolicy, timeoutPolicy, circuitBreakerPolicy);
        }
    }
}
