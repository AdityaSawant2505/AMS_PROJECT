using BLL.Interfaces;
using DLL.Interfaces;
using DLL.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class AuthManagerService : IAuthManagerService
    {
        private readonly IAuthService _authService;

        public AuthManagerService(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<AuthResponse> Authenticate(string username, string password)
        {
            return await _authService.Authenticate(username, password);
        }
    }
}
