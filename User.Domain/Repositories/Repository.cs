using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Domain.Exceptions.Users;
using User.Domain.Models;
using User.Domain.Repositories;

namespace User.Domain.Repositories;

public class Repository(DataContext context) : IRepository
{
    private readonly DataContext _context = context;
    #region Users
    public async Task<Users> AddUserAsync(Users users)
    {
        await _context.AddAsync(users);
        await _context.SaveChangesAsync();
        return users;
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Users> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id) ?? throw new UsersIdNotFoundException();
    }

    public async Task<List<Users>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Users> UpdateUserAsync(Users users)
    {
        _context.Users.Update(users);
        await _context.SaveChangesAsync();
        return users;
    }
    #endregion
}
