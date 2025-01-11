using BLL.Interfaces;
using DLL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class EmailManagerService : IEmailManagerService
    {
        private readonly IEmailService _emailService;

        public EmailManagerService(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task SendEmailAsync(List<string> receivers, string subject, string message, List<IFormFile> attachments = null)
        {
             await _emailService.SendEmailAsync(receivers, subject, message,attachments);
        }
    }
}
