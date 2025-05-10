using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace EShop.Domain.Seeders;
public class EShopSeeder(DataContext context) : IEShopSeeder
{
	public async Task Seed() 
	{
		// Sprawdzanie czy baza danych istnieje
		await context.Database.EnsureCreatedAsync();
		// Sprawdzenie czy tabela jest pusta
		if (!context.Products.Any())
		{
			var Products = new List<Product>
			{
				new() {
						Name = "samsung 1",
						Ean = "1234567890123",
						Price = 9.99m,
						Stock = 100,
						Sku = "SKU001",
						Category = new Category { Name = "Category 1" }
					},
				new() {
						Name = "ajfon 1",
						Ean = "2345678900123",
						Price = 999.99m,
						Stock = 10,
						Sku = "SKU021",
						Category = new Category { Name = "Category 1" }
					},
				new() {
						Name = "Laptop",
						Ean = "8900123890123",
						Price = 12.0m,
						Stock = 30,
						Sku = "SKU99",
						Category = new Category { Name = "Category 2" }
					},
			};

			context.Products.AddRange(Products);
			context.SaveChanges();
		}
	}
}
