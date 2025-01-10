using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailManagerService _emailManagerService;

        public EmailController(IEmailManagerService emailManagerService)
        {
            _emailManagerService = emailManagerService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromForm] List<string> receivers,
    [FromForm] string subject,
    [FromForm] string message,
    [FromForm] List<IFormFile> attachments)
        {
            try
            {
                await _emailManagerService.SendEmailAsync(receivers, subject, message, attachments);
                return Ok("Email sent successfully!");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while sending the email.");
            }
        }


    }
}
