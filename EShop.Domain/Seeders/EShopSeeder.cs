using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Seeders
{
    public class EShopSeeder(DataContext context) : IEShopSeeder
    {
        public async Task Seed()
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new() { Name = "Przygodowe-Fantasy" },
                    new() { Name = "Rodzinne" },
                    new() { Name = "Zręcznościowe" },
                    new() { Name = "Karciane" },
                    new() { Name = "Ekonomiczno-handlowe" },
                    new() { Name = "Strategiczne" },
                    new() { Name = "Imprezowe" }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
            if (!context.Products.Any())
            {
                var category = await context.Categories
                        .Where(x => x.Name == "Przygodowe-Fantasy").FirstOrDefaultAsync();

                var products = new List<Product>
                {
                    new() { Name = "Wiedźmin: Stary Świat", Ean = "14781", Category = category },
                    new() { Name = "Nemesis: Lockdown", Ean = "15000", Category = category },
                    new() { Name = "Heroes of Might and Magic III: Gra planszowa", Ean = "17580", Category = category }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
