using System.Net.Mail;
using System.Net;
using AuthenticationService.Infrastructure.Interfaces.Services;
using AuthenticationService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using AuthenticationService.Domain.Models;

namespace AuthenticationService.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings { get; }

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(Email email)
        {
            using var message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(_emailSettings.FromAddress, _emailSettings.FromName),
                Subject = email.Subject,
                Body = email.Body
            };

            foreach (var toEmail in email.To)
            {
                message.To.Add(new MailAddress(toEmail));
            }

            foreach (var ccEmail in email.CC ?? new List<string>())
            {
                message.CC.Add(new MailAddress(ccEmail));
            }

            foreach (var bccEmail in email.BCC ?? new List<string>())
            {
                message.Bcc.Add(new MailAddress(bccEmail));
            }

            var attachments = new List<Attachment>();

            try
            {
                if (email.FileAttachments != null && email.FileAttachments.Any())
                {
                    foreach (var fileAttachment in email.FileAttachments)
                    {
                        if (fileAttachment.Content.Length > 0)
                        {
                            var memoryStream = new MemoryStream(fileAttachment.Content);
                            var attachment = new Attachment(memoryStream, fileAttachment.FileName);
                            attachments.Add(attachment);
                            message.Attachments.Add(attachment);
                        }
                    }
                }

                using var smtpClient = new SmtpClient(_emailSettings.Server, _emailSettings.Port)
                {
                    Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password),
                    EnableSsl = _emailSettings.EnableSsl
                };

                await smtpClient.SendMailAsync(message);
            }
            finally
            {
                foreach (var attachment in attachments)
                {
                    attachment.Dispose();
                }
            }
        }
    }
}
