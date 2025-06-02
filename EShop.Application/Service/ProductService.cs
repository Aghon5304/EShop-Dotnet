using System.Text.Json;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
namespace EShop.Application.Service;

public class ProductService : IProductService
{
	private readonly IRepository _repository;
    private readonly IDatabase _redisdb;
    private readonly IMemoryCache _cache;
    public ProductService(IRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
        var redis = ConnectionMultiplexer.Connect("redis:6379");
        _redisdb = redis.GetDatabase();
    }
    public async Task<Product> AddAsync(Product product)
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
        string key = $"Product:{id}";
        string? productJson = await _redisdb.StringGetAsync(key);
        if (string.IsNullOrEmpty(productJson))
        {
            var product = await _repository.GetProductByIdAsync(id);
            await _redisdb.StringSetAsync(key, JsonSerializer.Serialize(product), TimeSpan.FromDays(1));
            return product;
        }
        else
        {
            var product = JsonSerializer.Deserialize<Product?>(productJson);
            return product;
        }
    }

    public async Task<Product> UpdateAsync(Product product)
	{
		var result = await _repository.UpdateProductAsync(product);
        string key = $"Product:{product.Id}";
        await _redisdb.KeyDeleteAsync(key);

        return result;
    }

    public async Task Delete(int id)
    {
        await _repository.DeleteProductAsync(id);
        await _redisdb.KeyDeleteAsync($"Product:{id}");
    }
}
