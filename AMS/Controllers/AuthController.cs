using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManagerService _authManagerService;

        public AuthController(IAuthManagerService authManagerService)
        {
            _authManagerService = authManagerService;
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            var authResponse = await _authManagerService.Authenticate(username, password);

            if (authResponse == null)
            {
                return Unauthorized(new { message = "Authentication failed" });
            }

            return Ok(authResponse);
        }
    }
}
