using DAL.Interfaces;
using DAL.Models.Users;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implimentation
{
    public class UsersService : IUsersService
    {
        string _connectionString;
        
        public UsersService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }
        public async Task<List<GetUsers>> GetUserByName(string name)
        {
            List<GetUsers> emp;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userName", name);

                await sqlConnection.OpenAsync();
                emp = (await sqlConnection.QueryAsync<GetUsers>(
                    "select * from Users where user_name=@userName",
                    parameters,
                    commandType: System.Data.CommandType.Text)).ToList();
            }

            return emp;
        }
    }
}
