using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Application.Services
{
    public interface IManageUsersService
    {
        public Task<List<Users>> GetAllAsync();
        Task<Users> GetAsync(int id);
        Task<Users> Update(Users users);
        Task<Users> Add(Users users);
        Task Delete(int id);
    }
}
