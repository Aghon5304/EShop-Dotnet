using User.Domain.Models;
using User.Domain.Repositories;

namespace User.Application.Services;

public class ManageUsersService(IRepository repository) : IManageUsersService
{
    private readonly IRepository _repository = repository;
    public async Task<Users> Add(Users users)
    {
        return await _repository.AddUserAsync(users);
    }

    public async Task<List<Users>> GetAllAsync()
    {
        return await _repository.GetUsersAsync();
    }

    public async Task<Users> GetAsync(int id)
    {
        return await _repository.GetUserByIdAsync(id);
    }

    public async Task<Users> Update(Users users)
    {
        return await _repository.UpdateUserAsync(users);
    }

    public async Task Delete(int id)
    {
        await _repository.DeleteUserAsync(id);
    }
}