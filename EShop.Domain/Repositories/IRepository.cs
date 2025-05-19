using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Repositories;

public interface IRepository
{
	#region Product
	Task<Product> GetProductByIdAsync(int id);
	Task<List<Product>> GetProductsAsync();
	Task<Product> AddProductAsync(Product product);
	Task<Product> UpdateProductAsync(int id, Product product);
	Task DeleteProductAsync(int id);
    #endregion
    #region Category
    Task<Category> GetCategoryByIdAsync(int id);
    Task<List<Category>> GetCategoryAsync();
    Task<Category> AddCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(int id, Category category);
    Task DeleteCategoryAsync(int id);
    #endregion
}