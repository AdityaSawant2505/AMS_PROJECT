using DLL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUsersManagerService
    {
        Task<GetUsers> GetUserByName(string name);
    }
}
