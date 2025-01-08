using BLL.Services.Interface;
using DAL.Interfaces;
using DAL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implimentation
{
    public class UserManager:IUserManager
    {
      private  IUsersService _usersService;

        public UserManager(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<List<GetUsers>> GetUserByName(string name)
        {
            return await _usersService.GetUserByName(name);
        }
    }
}
