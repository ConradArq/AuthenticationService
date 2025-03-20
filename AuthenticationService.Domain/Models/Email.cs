using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Models
{
    public class Email
    {
        public List<string> To { get; set; } = new List<string>();
        public List<string>? CC { get; set; }
        public List<string>? BCC { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public List<FileAttachment>? FileAttachments { get; set; }
    }

    public class FileAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] Content { get; set; } = new byte[0];
    }
}
