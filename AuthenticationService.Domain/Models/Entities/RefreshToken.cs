using AuthenticationService.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Models.Entities
{
    [Discoverable]
    public class RefreshToken : BaseDomainModel
    {
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public bool IsRevoked { get; set; } = false;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ReplacedByToken { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }
}
