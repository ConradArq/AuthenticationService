using System.Security.Cryptography;
using System.Text.Json;
using AuthenticationService.Shared.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;

public class OtpService : IOtpService
{
    private readonly IDistributedCache _cache;
    private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    public OtpService(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Generates, stores, and returns an OTP with its expiration time in minutes.
    /// </summary>
    public async Task<(string Otp, int ExpirationMinutes)> GenerateAndStoreOtpAsync(string userId, int expirationMinutes = 5)
    {
        var otp = GenerateOtp();

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes)
        };

        var otpData = JsonSerializer.Serialize(new { Otp = otp, ExpirationMinutes = expirationMinutes });
        await _cache.SetStringAsync($"OTP_{userId}", otpData, options);

        return (otp, expirationMinutes);
    }

    /// <summary>
    /// Retrieves OTP and expiration time (in minutes) from Redis.
    /// </summary>
    public async Task<(string? Otp, int? ExpirationMinutes)> GetOtpAsync(string userId)
    {
        var otpJson = await _cache.GetStringAsync($"OTP_{userId}");
        if (otpJson == null)
            return (null, null);

        var otpData = JsonSerializer.Deserialize<OtpData>(otpJson);
        return otpData is null ? (null, null) : (otpData.Otp, otpData.ExpirationMinutes);
    }

    /// <summary>
    /// Validates an OTP by comparing it to the stored value.
    /// </summary>
    public async Task<bool> ValidateOtpAsync(string userId, string enteredOtp)
    {
        var (storedOtp, expirationMinutes) = await GetOtpAsync(userId);

        if (storedOtp == null || expirationMinutes == null)
            return false;

        // Check if OTP has expired
        var cacheEntry = await _cache.GetStringAsync($"OTP_{userId}");
        if (cacheEntry == null) // If it's null, Redis already removed it due to expiration
            return false;

        if (storedOtp != enteredOtp)
            return false;

        // OTP is valid; remove it to prevent reuse
        await RemoveOtpAsync(userId);
        return true;
    }

    /// <summary>
    /// Deletes OTP after use.
    /// </summary>
    public async Task RemoveOtpAsync(string userId)
    {
        await _cache.RemoveAsync($"OTP_{userId}");
    }

    /// <summary>
    /// Checks if an OTP exists for a user.
    /// </summary>
    public async Task<bool> HasActiveOtpAsync(string userId)
    {
        return await _cache.GetStringAsync($"OTP_{userId}") != null;
    }

    /// <summary>
    /// Generates a secure OTP.
    /// </summary>
    private string GenerateOtp(int length = 6)
    {
        if (length < 4 || length > 10)
            throw new ArgumentOutOfRangeException(nameof(length), "OTP length should be between 4 and 10 digits.");

        Span<byte> bytes = stackalloc byte[length];
        _rng.GetBytes(bytes);

        return string.Concat(bytes.ToArray().Select(b => (b % 10).ToString()));
    }

    /// <summary>
    /// Represents OTP data stored in Redis.
    /// </summary>
    private class OtpData
    {
        public string Otp { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; }
    }
}