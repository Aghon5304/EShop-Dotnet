using System.Text.Json;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using StackExchange.Redis;
namespace EShop.Application.Service;

public class ProductService : IProductService
{
	private readonly IRepository _repository;
    private readonly IDatabase _redisdb;
    public ProductService(IRepository repository)
    {
        _repository = repository;
        var redis = ConnectionMultiplexer.Connect("localhost:6379");
        _redisdb = redis.GetDatabase();
    }
    public async Task<Product> Add(Product product)
	{
		var result = await _repository.AddProductAsync(product);
		await _redisdb.KeyDeleteAsync("products:all");
        return result;
	}
    public async Task<List<Product>> GetAllAsync()
	{
        var cached = await _redisdb.StringGetAsync("products:all");
        if (cached.HasValue)
        {
            return JsonSerializer.Deserialize<List<Product>>(cached)!;
        }
		else
		{
            var products = await _repository.GetProductsAsync();
            await _redisdb.StringSetAsync("products:all", JsonSerializer.Serialize(products));
            return products;
        }
    }

	public async Task<Product> GetAsync(int id)
	{
		var cached = await _redisdb.StringGetAsync($"products:{id}");
        if (cached.HasValue)
        {
            return JsonSerializer.Deserialize<Product>(cached)!;
        }
		else
		{
            var products = await _repository.GetProductByIdAsync(id);
            await _redisdb.StringSetAsync($"products:{id}", JsonSerializer.Serialize(products));
            return products;
        }
	}

	public async Task<Product> Update(int id, Product product)
	{
		var result = await _repository.UpdateProductAsync(id, product);
        await _redisdb.KeyDeleteAsync("products:all");
        return result;
	}

    public async Task Delete(int id)
    {
        await _repository.DeleteProductAsync(id);
        await _redisdb.KeyDeleteAsync($"products:{id}");
    }
}
