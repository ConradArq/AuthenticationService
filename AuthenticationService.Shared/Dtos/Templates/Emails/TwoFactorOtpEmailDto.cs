using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Shared.Dtos.Templates.Emails
{
    public class TwoFactorOtpEmailDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string Otp { get; set; } = string.Empty;
        public string ExpirationMinutes { get; set; } = string.Empty;
    }
}
