using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Domain.Exceptions.Login;
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
        var user = new User.Domain.Models.Entities.User { Username = users.Username, Email = users.Email, PasswordHash = users.PasswordHash, Roles = new List<Role> { _context.Role.First(r => r.Name == "User") } };
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();
        return users;
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.User
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new UserIdNotFoundException();

        user.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task<UserResponseDTO> GetUserByIdAsync(int id)
    {
        return await _context.User
            .Where(x => x.Id == id)
            .Select(x => new UserResponseDTO
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                CreatedAt = x.CreatedAt,
                LastLoginAt = x.LastLoginAt
            })
            .FirstOrDefaultAsync() ?? throw new UserIdNotFoundException(); 
    }

    public async Task<List<UserResponseDTO>> GetUserAsync()
    {
          return await _context.User
            .Select(x => new UserResponseDTO
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                CreatedAt = x.CreatedAt,
                LastLoginAt = x.LastLoginAt
            }).ToListAsync();
    }
    public async Task<UserLoginDTO> GetUserLoginAsync(string email)
    {
        return await _context.User
            .Where(x => x.Email == email)
            .Select(x => new UserLoginDTO
            {
                Id = x.Id,
                Email = x.Email,
                PasswordHash = x.PasswordHash,
                Roles = x.Roles
            })
            .FirstOrDefaultAsync() ?? throw new NoUserWithEmail();
    }
    public async Task<UserUpdateDTO> UpdateUserAsync(UserUpdateDTO users)
    {
        var userEntity = await _context.User
            .FirstOrDefaultAsync(u => u.Id == users.Id);

        if (userEntity == null)
            throw new UserIdNotFoundException();

        userEntity.Username = users.Username ?? userEntity.Username;
        userEntity.Email = users.Email ?? userEntity.Email;
        userEntity.PasswordHash = users.PasswordHash ?? userEntity.PasswordHash;

        if (users.Roles != null)
        {
            userEntity.Roles = null;
            foreach (var role in users.Roles)
            {
                var attachedRole = await _context.Role.FindAsync(role.Id) ?? role;
                userEntity.Roles.Add(attachedRole);
            }
        }

        await _context.SaveChangesAsync();
        return users;
    }
    #endregion
}
