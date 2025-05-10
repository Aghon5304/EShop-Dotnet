using EShop.Domain.Models;
using EShop.Domain.Repositories;

namespace EShop.Application.Service;

public class ProductService(IRepository repository) : IProductService
{
	private readonly IRepository _repository = repository;

	public async Task<Product> Add(Product product)
	{
		var result = await _repository.AddProductAsync(product);
		return result;
	}

	public async Task<List<Product>> GetAllAsync()
	{
		var result = await _repository.GetProductsAsync();
		return result;
	}

	public async Task<Product> GetAsync(int id)
	{
		var result = await _repository.GetProductByIdAsync(id);
		return result;
	}

	public async Task<Product> Update(Product product)
	{
		var result = await _repository.UpdateProductAsync(product);
		return result;
	}
}
