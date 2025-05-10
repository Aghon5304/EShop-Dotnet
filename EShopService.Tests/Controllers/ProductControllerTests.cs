using EShopService.Controllers;
using EShop.Application.Service;
using EShop.Domain.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EShopService.Tests.Controllers;

public class ProductControllerTests
{
	private readonly Mock<IProductService> _productServiceMock;
	private readonly ProductController _productController;

	public ProductControllerTests()
	{
		_productServiceMock = new Mock<IProductService>();
		_productController = new ProductController(_productServiceMock.Object);
	}

	[Fact]
	public async Task GetAll_ShouldReturnAllProducts_ReturnTrue()
	{
		var products = new List<Product>
		{
			new() { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 12 },
			new() { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 1 }
		};

		_productServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(products);
		
		var result = await _productController.Get();

		Assert.Equal(products, Assert.IsType<OkObjectResult>(result).Value);
	}
	[Fact]
	public async Task GetById_ShouldReturnCorrectProduct_ReturnTrue()
	{
		var products = new List<Product>
		{
			new() { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 12 },
			new() { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 1 }
		};
		
		_productServiceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(products[0]);

		var result = await _productController.Get(1);

		Assert.Equal(products[0], Assert.IsType<OkObjectResult>(result).Value);
	}
	[Fact]
	public async Task GetById_IdIncorrect_ReturnError()
	{
		var products = new List<Product>
		{
			new() { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 12 },
			new() { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 1 }
		};

		_productServiceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(products[0]);

		var result = await _productController.Get(99);

		Assert.IsNotType<OkObjectResult>(result);
	}
	[Fact]
	public async Task CreateProduct_ShouldCreateNewProduct_ReturnTrue()
	{
		var products = new List<Product>
		{
			new() { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 12 },
			new() { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 1 }
		};

		_productServiceMock.Setup(s => s.Add(It.IsAny<Product>())).ReturnsAsync(products[0]);
		var result = await _productController.Post(products[0]);
		Assert.Equal(products[0], Assert.IsType<OkObjectResult>(result).Value);
	}
	[Fact]
	public async Task EditProduct_ShouldEditProduct_ReturnTrue()
	{
		var products = new List<Product>
		{
			new() { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 12 },
			new() { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 1 }
		};
		_productServiceMock.Setup(s => s.Update(It.IsAny<Product>())).ReturnsAsync(products[0]);
		var result = await _productController.Put(1, products[0]);
		Assert.Equal(products[0], Assert.IsType<OkObjectResult>(result).Value);
	}
	[Fact]
	public async Task Delete_ShouldDeleteProduct_ReturnTrue()
	{
		var products = new List<Product>
		{
			new() { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 12 },
			new() { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 1 }
		};
		_productServiceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(products[0]);
		_productServiceMock.Setup(s => s.Update(It.IsAny<Product>())).ReturnsAsync(products[0]);
		var result = await _productController.Delete(1);
		Assert.Equal(products[0], Assert.IsType<OkObjectResult>(result).Value);
	}
}
