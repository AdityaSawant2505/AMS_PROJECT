using BLL.Services.Interface;
using DAL.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserManager _userManager;
        private UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        [Route("GetUserByName")]
        public async Task<List<GetUsers>>GetUserByName(string userName)
        {
            return await _userManager.GetUserByName(userName);
        }
    }
}
