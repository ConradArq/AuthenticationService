using AuthenticationService.Application.Features.ApplicationRole;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.ApplicationUser
{
    public class ApplicationUserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public bool IsRegistered { get; set; }
        public string? UserName { get; set; }
        public List<string> RoleNames { get; set; } = new();
        public int StatusId { get; set; }
    }
}
