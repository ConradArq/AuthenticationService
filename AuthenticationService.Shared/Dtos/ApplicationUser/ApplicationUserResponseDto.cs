
namespace AuthenticationService.Shared.Dtos.ApplicationUser
{
    public class ApplicationUserResponseDto
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
