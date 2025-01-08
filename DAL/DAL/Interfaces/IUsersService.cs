using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.Users;

namespace DAL.Interfaces
{
    public interface IUsersService
    {
         Task<List<GetUsers>> GetUserByName(string  name);

    }
}
