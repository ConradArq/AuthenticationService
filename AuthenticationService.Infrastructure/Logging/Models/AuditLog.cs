using AuthenticationService.Infrastructure.Logging.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Logging.Models
{
    public class AuditLog: BaseLogModel
    {
        public EventType EventType { get; set; }
        public string? RequestUrl { get; set; }
        public string? EntityName { get; set; }
        public string? Message { get; set; }
        public string? OldData { get; set; }
        public string? NewData { get; set; }
    }
}
