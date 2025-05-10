using DAL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interface
{
    public interface IUserManager
    {
        Task<List<GetUsers>> GetUserByName(string name);

    }
}
