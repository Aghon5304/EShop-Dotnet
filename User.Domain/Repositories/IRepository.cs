using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Domain.Repositories;
public interface IRepository
{
    #region Users
    Task<Users> GetUserByIdAsync(int id);
    Task<List<Users>> GetUsersAsync();
    Task<Users> AddUserAsync(Users users);
    Task<Users> UpdateUserAsync(Users users);
    Task DeleteUserAsync(int id);
    #endregion
}
