using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Domain.Exceptions.User;
using User.Domain.Models.Entities;
using User.Domain.Models.Response;
using User.Domain.Repositories;

namespace User.Domain.Repositories;

public class Repository(DataContext context) : IRepository
{
    private readonly DataContext _context = context;
    #region User
    public async Task<UserCreateDTO> AddUserAsync(UserCreateDTO users)
    {
        await _context.AddAsync(users);
        await _context.SaveChangesAsync();
        return users;
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<UserResponseDTO> GetUserByIdAsync(int id)
    {
        return await _context.User.FindAsync(id) ?? throw new UserIdNotFoundException();
    }

    public async Task<List<UserResponseDTO>> GetUserAsync()
    {
        return await _context.User.ToListAsync();
    }

    public async Task<UserUpdateDTO> UpdateUserAsync(UserUpdateDTO users)
    {
        _context.User.Update(users);
        await _context.SaveChangesAsync();
        return users;
    }
    #endregion
}
