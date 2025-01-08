using Dapper;
using DLL.Interfaces;
using DLL.Models.Users;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly string _connectionString;
        public UsersService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Myconn");
        }
        public async Task<GetUsers> GetUserByName(string name)
        {
            GetUsers usr;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userName", name);
                var query = @"
            SELECT 
                user_id AS UserId,
                user_hash AS UserHash,
                user_name AS UserName,
                user_email AS UserEmail,
                department AS Department,
                designation AS Designation,
                manager_id AS ManagerId,
                password_hash AS PasswordHash,
                password_salt AS PasswordSalt,
                is_deleted AS IsDeleted,
                created_by AS CreatedBy,
                created_on AS CreatedOn,
                updated_by AS UpdatedBy,
                updated_on AS UpdatedOn,
                refresh_token AS RefreshToken,
                reset_token AS ResetToken,
                reset_token_expiry AS ResetTokenExpiry,
                phone_number AS PhoneNumber,
                otp AS Otp,
                otp_create_time AS OtpCreateTime
            FROM Users
            WHERE user_name = @userName";


                await sqlConnection.OpenAsync();
                usr = (await sqlConnection.QueryFirstOrDefaultAsync<GetUsers>(
                    query,
                    parameters,
                    commandType: System.Data.CommandType.Text));
            }

            return usr;
        }
    }
}
