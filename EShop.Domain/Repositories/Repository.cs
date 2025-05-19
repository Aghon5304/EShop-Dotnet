using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Repositories;

public class Repository(DataContext context) : IRepository
{
	// Implementacja repozytorium	
	private readonly DataContext _context = context;

    #region Category
    public async Task<Category> AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
    public async Task DeleteCategoryAsync(int id)
    {
        _context.Categories.Remove(new Category { Id = id });
        await _context.SaveChangesAsync();
    }
    public async Task<List<Category>> GetCategoryAsync()
    {
        return await _context.Categories.ToListAsync();
    }
    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }
    public async Task<Category> UpdateCategoryAsync(int id, Category category)
    {
        var existingCategory = await _context.Categories.FindAsync(id) ?? throw new Exception("Category not found");
        existingCategory = category;
        await _context.SaveChangesAsync();
        return existingCategory;
    }
    #endregion

    #region Product
    public async Task<Product> AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }
    public async Task DeleteProductAsync(int id)
    {
        _context.Products.Remove(new Product { Id = id });
        await _context.SaveChangesAsync();
    }
    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);

    }
    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<Product> UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _context.Products.FindAsync(id) ?? throw new Exception("Product not found");
        existingProduct = product;
        await _context.SaveChangesAsync();
        return existingProduct;
    }
    #endregion
}
