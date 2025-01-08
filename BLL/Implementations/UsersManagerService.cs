using BLL.Interfaces;
using DLL.Interfaces;
using DLL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class UsersManagerService : IUsersManagerService
    {
        private readonly IUsersService _usersService;

        public UsersManagerService(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<GetUsers> GetUserByName(string name)
        {
            return await _usersService.GetUserByName(name);
        }
    }
}
