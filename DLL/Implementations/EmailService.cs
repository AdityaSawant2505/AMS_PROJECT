using DLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

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
            var server = _configuration["SMTP:Server"];
            var port = int.Parse(_configuration["SMTP:Port"]);
            var senderEmail = _configuration["SMTP:SenderEmail"];
            var senderPassword = _configuration["SMTP:SenderPassword"];

            // Configure the SMTP client
            using (var smtpClient = new SmtpClient(server, port))
            {
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true; // Assuming SSL/TLS is required

                foreach (var receiver in receivers)
                {
                    // Create a new email message for each recipient
                    var email = new MailMessage
                    {
                        From = new MailAddress(senderEmail),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = true // Assuming the message is in HTML format
                    };

                    // Add the current recipient
                    email.To.Add(receiver);

                    // Add attachments if any
                    if (attachments != null && attachments.Any())
                    {
                        foreach (var attachment in attachments)
                        {
                            using (var stream = new MemoryStream())
                            {
                                await attachment.CopyToAsync(stream);
                                var mailAttachment = new Attachment(new MemoryStream(stream.ToArray()), attachment.FileName);
                                email.Attachments.Add(mailAttachment);
                            }
                        }
                    }

                    try
                    {
                        // Send the email to the current recipient
                        await smtpClient.SendMailAsync(email);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending email to {receiver}: {ex.Message}");
                    }
                    finally
                    {
                        // Dispose of attachments to avoid memory leaks
                        email.Attachments.Dispose();
                    }
                }
            }
        }
    }
}
