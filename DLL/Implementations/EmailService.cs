using DLL.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace DLL.Implementations
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(List<string> receivers, string subject, string message, List<IFormFile> attachments = null)
        {
            // Get SMTP settings from configuration
            var server = _configuration["SMTP:Server"];
            var port = int.Parse(_configuration["SMTP:Port"]);
            var senderEmail = _configuration["SMTP:SenderEmail"];
            var senderPassword = _configuration["SMTP:SenderPassword"];

            // Create the email message
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("AMS", senderEmail));
            foreach (var receiver in receivers)
            {
                email.To.Add(new MailboxAddress("", receiver));
            }

            email.Subject = subject;

            // Create the email body with attachments
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };

            // Add attachments if any
            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    using (var stream = new MemoryStream())
                    {
                        await attachment.CopyToAsync(stream);
                        bodyBuilder.Attachments.Add(attachment.FileName, stream.ToArray());
                    }
                }
            }

            email.Body = bodyBuilder.ToMessageBody();

            // Send the email
            var smtpClient = new SmtpClient();
            try
            {
                await smtpClient.ConnectAsync(server, port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(senderEmail, senderPassword);
                await smtpClient.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
            }
        }

    }

}
