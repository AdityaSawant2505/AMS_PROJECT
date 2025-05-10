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
using System.Xml.Linq;

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

        public async Task<List<string>> GetPermissions(long userId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userid", userId);

                await sqlConnection.OpenAsync();

                // Execute the stored procedure and fetch the result as a list of strings
                var permissions = (await sqlConnection.QueryAsync<string>(
                    "GetPermissionsByUserId", // Stored Procedure Name
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure)).ToList();

                return permissions;
            }
        }


        public async Task<List<string>> GetRoles(long userId)
        {
            using(SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userId", userId);

                await sqlConnection.OpenAsync();
                var roles = (await sqlConnection.QueryAsync<string>(
                    "GetRolesByUserId", 
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure)).ToList();

                sqlConnection.Close();

                return roles;
            }
        }


        public async Task<bool> CreateUser(InsertUserRequest request)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userHash", request.UserHash);
                parameters.Add("@userName", request.UserName);
                parameters.Add("@userEmail", request.UserEmail);
                parameters.Add("@department", request.Department);
                parameters.Add("@designation", request.Designation);
                parameters.Add("@managerId", request.ManagerId);
                parameters.Add("@passwordHash", request.PasswordHash);
                parameters.Add("@passwordSalt", request.PasswordSalt);
                parameters.Add("@isDeleted", request.IsDeleted);
                parameters.Add("@createdBy", request.CreatedBy);
                parameters.Add("@createdOn", DateTime.UtcNow);
                parameters.Add("@updatedBy", request.UpdatedBy);
                parameters.Add("@updatedOn", DateTime.UtcNow);
                parameters.Add("@refreshToken", request.RefreshToken);
                parameters.Add("@resetToken", request.ResetToken);
                parameters.Add("@resetTokenExpiry", request.ResetTokenExpiry);
                parameters.Add("@phoneNumber", request.PhoneNumber);
                parameters.Add("@Otp", request.Otp);
                parameters.Add("@otpCreateTime", request.OtpCreateTime);

                // Use stored procedure instead of direct query
                var result = await sqlConnection.ExecuteAsync(
                    "CreateUser",  // Name of the stored procedure
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result > 0;
            }
        }


    }
}
