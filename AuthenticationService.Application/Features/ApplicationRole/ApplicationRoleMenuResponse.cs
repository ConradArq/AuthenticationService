using AuthenticationService.Application.Features.ApplicationMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.ApplicationRole
{
    public class ApplicationRoleMenuResponse
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public List<ApplicationMenuResponse> Menus { get; set; } = new();
    }
}
