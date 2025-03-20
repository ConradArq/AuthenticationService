namespace AuthenticationService.Shared.Interfaces.Services
{
    public interface IOtpService
    {
        /// <summary>
        /// Generates, stores, and returns an OTP with its expiration time in minutes.
        /// </summary>
        Task<(string Otp, int ExpirationMinutes)> GenerateAndStoreOtpAsync(string userId, int expirationMinutes = 5);

        /// <summary>
        /// Retrieves OTP and expiration time (in minutes) from Redis.
        /// </summary>
        Task<(string? Otp, int? ExpirationMinutes)> GetOtpAsync(string userId);

        /// <summary>
        /// Validates an OTP by comparing it to the stored value.
        /// </summary>
        Task<bool> ValidateOtpAsync(string userId, string enteredOtp);

        /// <summary>
        /// Deletes OTP after use.
        /// </summary>
        Task RemoveOtpAsync(string userId);

        /// <summary>
        /// Checks if an OTP exists for a user.
        /// </summary>
        Task<bool> HasActiveOtpAsync(string userId);
    }
}