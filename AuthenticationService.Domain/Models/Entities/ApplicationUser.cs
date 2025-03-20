using AuthenticationService.Domain.Interfaces.Models;
using AuthenticationService.Shared.Attributes;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Domain.Models.Entities
{
    [Discoverable]
    public class ApplicationUser : IdentityUser, IBaseDomainModel
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

        // Wrapper property to ensure that Email is always non-null when accessed via ApplicationUser.
        public new string Email
        {
            get => base.Email ?? string.Empty;
            set => base.Email = value;
        }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        // Google OAuth user ID.
        public string? GoogleId { get; set; }
        // Microsoft OAuth user ID.
        public string? MicrosoftId { get; set; }
        //Indicates if the user has completed regular registration. Should be false if the user was created through an OAuth flow.
        public bool IsRegistered { get; set; }

        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }

        public int StatusId { get; set; } = (int)Enums.Status.Active;
        public virtual Status Status { get; set; } = null!;

        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; } = new HashSet<ApplicationUserRole>();
    }
}

