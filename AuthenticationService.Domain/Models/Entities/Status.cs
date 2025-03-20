using AuthenticationService.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Models.Entities
{
    [Discoverable]
    public class Status: BaseDomainModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
