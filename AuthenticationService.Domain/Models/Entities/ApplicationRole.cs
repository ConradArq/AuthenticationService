using AuthenticationService.Domain.Interfaces.Models;
using AuthenticationService.Shared.Attributes;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Domain.Models.Entities
{
    [Discoverable]
    public class ApplicationRole : IdentityRole, IBaseDomainModel
    {
        // Explicitly implemented property to fulfill the contract of the IBaseDomainModel interface. It won't be mapped by EF
        // because explicit interface implementations are ignored during database schema generation.
        object IBaseDomainModel.Id
        {
            get => Id;
            set
            {
                if (value is string stringValue)
                    Id = stringValue;
                else
                    throw new InvalidOperationException("Id must be of type string.");
            }
        }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }

        public int StatusId { get; set; } = (int)Enums.Status.Active;
        public virtual Status Status { get; set; } = null!;

        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; } = new HashSet<ApplicationUserRole>();
        public virtual ICollection<ApplicationRoleMenu> ApplicationRoleMenus { get; set; } = new HashSet<ApplicationRoleMenu>();
    }
}