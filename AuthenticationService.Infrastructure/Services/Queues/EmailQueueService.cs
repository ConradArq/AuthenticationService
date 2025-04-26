using AuthenticationService.Domain.Models;
using AuthenticationService.Infrastructure.Interfaces.Services;
using AuthenticationService.Infrastructure.PollyPolicies;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

namespace AuthenticationService.Infrastructure.Services.Queues
{
    public class EmailQueueService : IEmailQueueService
    {
        private readonly Channel<Func<Task<Email>>> _channel;
        private readonly IDatabase _redisDb;
        private readonly IPollyPolicyRegistry _pollyPolicies;

        public EmailQueueService(IConnectionMultiplexer redis, IPollyPolicyRegistry pollyPolicies)
        {
            // Enable email throttling by limiting the channel to 100 queued (pending) items.
            // Producers will wait if the buffer is full, preventing overload and enforcing backpressure.
            var options = new BoundedChannelOptions(capacity: 100)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _channel = Channel.CreateBounded<Func<Task<Email>>>(options);

            _redisDb = redis.GetDatabase();

            _pollyPolicies = pollyPolicies;
        }

        public void EnqueueEmail(Func<Task<Email>> buildEmailFuncAsync)
        {
            _channel.Writer.TryWrite(buildEmailFuncAsync);
        }

        /// <summary>
        /// Attempts to enqueue an email to be sent, ensuring uniqueness across distributed systems using a Redis lock.
        /// </summary>
        /// <param name="emailBuilder">A function that asynchronously builds the email to send.</param>
        /// <param name="distributedKeyParts">
        /// A dictionary of key parts used to generate a unique distributed key (e.g., recipients and subject).
        /// If the Redis key already exists, the email will not be enqueued again.
        /// </param>
        /// <returns>
        /// True if the email was successfully enqueued; false if a duplicate exists and was skipped.
        /// </returns>
        public async Task<bool> EnqueueEmailAsync(Func<Task<Email>> emailBuilder, Dictionary<string, string> distributedKeyParts)
        {
            var distributedKey = GenerateDistributedKey(distributedKeyParts);
            var key = $"email:queued:{distributedKey}";

            bool wasSet = await _pollyPolicies.RedisRetryPolicy.ExecuteAsync(() => _redisDb.StringSetAsync(key, "1", TimeSpan.FromMinutes(5), When.NotExists));

            if (wasSet)
            {
                _channel.Writer.TryWrite(emailBuilder);
                return true;
            }

            return false;
        }

        public async Task<Func<Task<Email>>> DequeueEmailAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }

        private static string GenerateDistributedKey(Dictionary<string, string> parts)
        {
            var orderedParts = parts.OrderBy(x => x.Key)
                                    .Select(x => $"{x.Key}={x.Value}")
                                    .ToList();

            var rawKey = string.Join("|", orderedParts);

            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawKey));
            return Convert.ToHexString(hashBytes);
        }
    }
}