using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.ApplicationMenu
{
    public class ApplicationMenuResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Path { get; set; }
        public string? IconType { get; set; }
        public string? Icon { get; set; }
        public string? Class { get; set; }
        public bool? GroupTitle { get; set; }
        public string? Badge { get; set; }
        public string? BadgeClass { get; set; }
        public int Order { get; set; }

        public int? ParentApplicationMenuId { get; set; }

        public int StatusId { get; set; }

        public ICollection<ApplicationMenuResponse> SubMenus { get; set; } = new List<ApplicationMenuResponse>();
    }
}
