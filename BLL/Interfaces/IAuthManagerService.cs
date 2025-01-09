using DLL.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAuthManagerService
    {
        Task<AuthResponse> Authenticate(string username, string password);

    }
}
