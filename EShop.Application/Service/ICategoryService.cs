using EShop.Domain.Models;

namespace EShop.Application.Service;

public interface ICategoryService
{
	public Task<List<Category>> GetAllAsync();
	Task<Category> GetAsync(int id);
	Task<Category> Update(Category category);
	Task<Category> AddAsync(Category category);
	Task Delete(int id);
}
