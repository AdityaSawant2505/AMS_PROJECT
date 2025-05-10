using DLL.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Authenticate(string username, string password);

    }
}
