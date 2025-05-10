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

        public async Task<bool> CreateUser(InsertUserRequest request)
        {
            return await _usersService.CreateUser(request);
        }

        public async Task<List<string>> GetPermissions(long userId)
        {
            return await _usersService.GetPermissions(userId);
        }

        public async Task<List<string>> GetRoles(long userId)
        {
            return await _usersService.GetRoles(userId);
        }

        public async Task<GetUsers> GetUserByName(string name)
        {
            return await _usersService.GetUserByName(name);
        }
    }
}
