using AuthenticationService.Shared.Attributes;

namespace AuthenticationService.Domain.Models.Entities
{
    [Discoverable]
    public class ApplicationMenu : BaseDomainModel
    {
        public string Title { get; set; } = string.Empty;
        public string? Path { get; set; }
        public string? IconType { get; set; }
        public string? Icon { get; set; }
        public string? Class { get; set; }
        public bool? GroupTitle { get; set; }
        public string? Badge { get; set; }
        public string? BadgeClass { get; set; }
        public int Order { get; set; }

        public virtual Status Status { get; set; } = null!;

        public int? ParentApplicationMenuId { get; set; }
        public ApplicationMenu? ParentApplicationMenu { get; set; }

        public virtual ICollection<ApplicationMenu> ApplicationSubMenus { get; set; } = new HashSet<ApplicationMenu>();
        public virtual ICollection<ApplicationRoleMenu> ApplicationRoleMenus { get; set; } = new HashSet<ApplicationRoleMenu>();
    }
}