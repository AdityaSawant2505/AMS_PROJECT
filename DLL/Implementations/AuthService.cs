using DLL.Interfaces;
using DLL.Models.Auth;
using DLL.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;

        public AuthService(IUsersService usersService, IConfiguration configuration)
        {
            _usersService = usersService;
            _configuration = configuration;
        }
        public async Task<AuthResponse> Authenticate(string username, string password)
        {
            try
            {
                // Fetch user details from UsersService
                var user = await _usersService.GetUserByName(username);
                if (user == null || user.IsDeleted || username != user.UserName)
                    return null;

                // Verify password
                bool isPasswordValid = CheckPassword(user.PasswordHash, user.PasswordSalt, password);
                if (!isPasswordValid)
                    return null;

                // Fetch roles and permissions
                var roles = await _usersService.GetRoles(user.UserId);
                var permissions = await _usersService.GetPermissions(user.UserId);

                // Generate JWT token
                var authResponse = GenerateTokenResponse(user, roles, permissions);

                return authResponse;
            }
            catch (Exception ex)
            {
                // Log any errors and rethrow
                Console.Error.WriteLine($"Error in {nameof(Authenticate)}: {ex.Message}");
                throw;
            }
        }

        private bool CheckPassword(string hash, string salt, string password)
        {
            var hashKey = Convert.FromBase64String(hash);
            var saltKey = Convert.FromBase64String(salt);

            using (var algorithm = new Rfc2898DeriveBytes(password, saltKey, 10000, HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(64);
                return keyToCheck.SequenceEqual(hashKey);
            }
        }

        public async Task<bool> RegisterUser(InsertUserRequest request)
        {
            // Hash password
            var (hashedPassword, salt) = HashPassword(request.Password);

            // Set password hash and salt
            request.PasswordHash = hashedPassword;
            request.PasswordSalt = salt;

            // Call UsersService to insert the user into the database
            var result = await _usersService.CreateUser(request);
            return result;
        }

        public (string hashedPassword, string salt) HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[16]; // 128-bit salt
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA512))
                {
                    var hashedPassword = pbkdf2.GetBytes(64);
                    return (Convert.ToBase64String(hashedPassword), Convert.ToBase64String(salt));
                }
            }
        }

        // Generate JWT token with roles and permissions
        private AuthResponse GenerateTokenResponse(GetUsers user, List<string> roles, List<string> permissions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserEmail)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            claims.AddRange(permissions.Select(permission => new Claim("Permission", permission)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponse
            {
                Token = tokenHandler.WriteToken(token),
                CreatedOn = DateTime.UtcNow,
                Expires = tokenDescriptor.Expires,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpires = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["Jwt:RefreshTokenExpirationDays"]))
            };
        }

        // Generate Refresh Token
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
