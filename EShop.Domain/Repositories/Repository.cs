using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Repositories;

public class Repository : IRepository
{
	// Implementacja repozytorium	
	private readonly DataContext _context;

	public Repository(DataContext context)
	{
		_context = context;
	}

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

	public async Task<Product> UpdateProductAsync(Product product)
	{
		_context.Products.Update(product);
		await _context.SaveChangesAsync();
		return product;
	}
}
