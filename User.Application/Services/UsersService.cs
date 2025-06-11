using User.Domain.Models.Entities;
using User.Domain.Models.Response;
using User.Domain.Repositories;

namespace User.Application.Services;

public class UserService(IRepository repository) : IUserService
{
    private readonly IRepository _repository = repository;
    public async Task<UserCreateDTO> Add(UserCreateDTO users)
    {
        return await _repository.AddUserAsync(users);
    }

    public async Task<List<UserResponseDTO>> GetAllAsync()
    {
        return await _repository.GetUserAsync();
    }

    public async Task<UserResponseDTO> GetAsync(int id)
    {
        return await _repository.GetUserByIdAsync(id);
    }

    public async Task<UserUpdateDTO> Update(UserUpdateDTO users)
    {
        return await _repository.UpdateUserAsync(users);
    }

    public async Task Delete(int id)
    {
        await _repository.DeleteUserAsync(id);
    }
}