using DLL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Interfaces
{
    public interface IUsersService
    {
        Task<bool> CreateUser(InsertUserRequest request);
        Task<GetUsers> GetUserByName(string name);
        Task<List<string>> GetPermissions(long userId);
        Task<List<string>> GetRoles(long userId);
    }
}
