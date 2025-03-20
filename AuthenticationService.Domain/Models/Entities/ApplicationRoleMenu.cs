using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Models.Entities
{
    public class ApplicationRoleMenu : BaseDomainModel
    {
        public virtual Status Status { get; set; } = null!;

        public string ApplicationRoleId { get; set; } = null!;
        public virtual ApplicationRole ApplicationRole { get; set; } = null!;

        public int ApplicationMenuId { get; set; }
        public virtual ApplicationMenu ApplicationMenu { get; set; } = null!;
    }
}
