using System.Text.Json;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
namespace EShop.Application.Service;

public class CategoryService : ICategoryService
{
	private readonly IRepository _repository;
	private readonly IDatabase _redisdb;
    private readonly IMemoryCache _cache;
    public CategoryService(IRepository repository, IMemoryCache cache, IDatabase redisdb)
    { 
        _repository = repository;
        _cache = cache;
        _redisdb = redisdb;
    }
	public async Task<Category> AddAsync(string Name)
	{
        var category = new Category
        {
            Name = Name
        };
        var result = await _repository.AddCategoryAsync(category);

        return result;
	}
    public async Task<List<Category>> GetAllAsync()
    {
        var result = await _repository.GetCategoryAsync();

        return result;
    }

    public async Task<Category> GetAsync(int id)
	{
        string key = $"category:{id}";
        var cached = await _redisdb.StringGetAsync(key);
        if (string.IsNullOrEmpty(cached))
        {
            var categories = await _repository.GetCategoryByIdAsync(id);
            await _redisdb.StringSetAsync(key, JsonSerializer.Serialize(categories), TimeSpan.FromDays(1));
            return categories;
        }
        else
        {
            var categories = JsonSerializer.Deserialize<Category?>(cached);
            return categories;
        }
    }

	public async Task<Category> Update(Category category)
	{
		var result = await _repository.UpdateCategoryAsync(category);
        await _redisdb.KeyDeleteAsync("category:all");
        return result;
	}
    public async Task Delete(int id)
    {
        await _repository.DeleteCategoryAsync(id);
        await _redisdb.KeyDeleteAsync($"category:{id}");
    }
}
