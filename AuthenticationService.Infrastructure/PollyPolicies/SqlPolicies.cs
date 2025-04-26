using Microsoft.Data.SqlClient;
using Polly.Retry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.PollyPolicies
{
    public static class SqlPolicies
    {
        public static AsyncRetryPolicy GetSqlRetryPolicy()
        {
            return Policy
                .Handle<SqlException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(1),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        // TODO: Implement custom logging
                        Console.WriteLine($"[SQL] Retry {retryCount} after {timeSpan.TotalSeconds}s due to {exception.GetType().Name}");
                    });
        }
    }
}
