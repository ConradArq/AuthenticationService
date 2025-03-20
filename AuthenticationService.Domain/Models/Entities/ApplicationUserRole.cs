using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Models.Entities
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }

        public int StatusId { get; set; } = (int)Enums.Status.Active;
        public virtual Status Status { get; set; } = null!;

        public virtual ApplicationRole ApplicationRole { get; set; } = null!;
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
