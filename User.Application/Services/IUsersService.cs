using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Entities;
using User.Domain.Models.Response;

namespace User.Application.Services
{
    public interface IUserService
    {
        public Task<List<UserResponseDTO>> GetAllAsync();
        Task<UserResponseDTO> GetAsync(int id);
        Task<UserLoginDTO> GetLoginAsync(string email);
        Task<UserUpdateDTO> Update(UserUpdateDTO users);
        Task<UserUpdatePasswordDTO> UpdatePassword(UserUpdatePasswordDTO users);
        Task<UserCreateDTO> Add(UserCreateDTO users);
        Task Delete(int id);
    }
}
