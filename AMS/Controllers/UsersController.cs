using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;

namespace AMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersManagerService _usersManagerService;
        public UsersController(IUsersManagerService usersManagerService)
        {
            _usersManagerService = usersManagerService;
        }


        [HttpGet("GetUsersByName")]

        public async Task<IActionResult> GetUsersByName(string UsersName)
        {
            var result = await _usersManagerService.GetUserByName(UsersName);
            return Ok(result);
        }
    }
}
