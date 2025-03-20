using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Logging.Models
{
    public class ErrorLog: BaseLogModel
    {
        public DateTime ErrorDate { get; set; }
        public string ExceptionType { get; set; } = string.Empty;
        public string ExceptionMessage { get; set; } = string.Empty;
        public string ErrorStackTrace { get; set; } = string.Empty;
    }
}
