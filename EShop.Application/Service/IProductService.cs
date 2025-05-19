using EShop.Domain.Models;

namespace EShop.Application.Service;

public interface IProductService
{
	public Task<List<Product>> GetAllAsync();
	Task<Product> GetAsync(int id);
	Task<Product> Update(int id, Product product);
	Task<Product> Add(Product product);
	Task Delete(int id);
}
