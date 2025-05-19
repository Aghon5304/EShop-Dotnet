using System.Text.Json;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
namespace EShop.Application.Service;

public class CategoryService(IRepository repository, IConnectionMultiplexer redis) : ICategoryService
{
	private readonly IRepository _repository = repository;
	private readonly IDatabase _redisdb = redis.GetDatabase();

    public async Task<Category> Add(Category category)
	{
        var result = await _repository.AddCategoryAsync(category);
		await _redisdb.KeyDeleteAsync("category:all");
        return result;
	}
    public async Task<List<Category>> GetAllAsync()
	{
        var cached = await _redisdb.StringGetAsync("category:all");
        if (cached.HasValue)
        {
            return JsonSerializer.Deserialize<List<Category>>(cached)!;
        }
		else
		{
            var categories = await _repository.GetCategoryAsync();
            await _redisdb.StringSetAsync("category:all", JsonSerializer.Serialize(categories));
            return categories;
        }
    }

	public async Task<Category> GetAsync(int id)
	{
		var cached = await _redisdb.StringGetAsync($"category:{id}");
        if (cached.HasValue)
        {
            return JsonSerializer.Deserialize<Category>(cached)!;
        }
		else
		{
            var categories = await _repository.GetCategoryByIdAsync(id);
            await _redisdb.StringSetAsync($"category:{id}", JsonSerializer.Serialize(categories));
            return categories;
        }
	}

	public async Task<Category> Update(int id, Category category)
	{
		var result = await _repository.UpdateCategoryAsync(id, category);
        await _redisdb.KeyDeleteAsync("category:all");
        return result;
	}
    public async Task Delete(int id)
    {
        await _repository.DeleteCategoryAsync(id);
        await _redisdb.KeyDeleteAsync($"category:{id}");
    }
}
