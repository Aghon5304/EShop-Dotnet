using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Entities;
using User.Domain.Models.Response;

namespace User.Domain.Repositories;
public interface IRepository
{
    #region User
    Task<UserResponseDTO> GetUserByIdAsync(int id);
    Task<List<UserResponseDTO>> GetUserAsync();
    Task<UserCreateDTO> AddUserAsync(UserCreateDTO users);
    Task<UserUpdateDTO> UpdateUserAsync(UserUpdateDTO users);
    Task<UserUpdatePasswordDTO> UpdateUserPasswordAsync(UserUpdatePasswordDTO users);
    Task<UserLoginDTO> GetUserLoginAsync(string email);
    Task<UserUpdateLoginAtDTO> UpdateUserLastLogIn(UserUpdateLoginAtDTO users);
    Task DeleteUserAsync(int id);
    #endregion
}
