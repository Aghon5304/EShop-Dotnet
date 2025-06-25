using EShop.Application.Service;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using StackExchange.Redis;
using System.Text.Json;

namespace EShop.Application.Tests.Service;

public class ProductServiceTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<IDatabase> _redisMock;
    private readonly Mock<IConnectionMultiplexer> _connectionMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _cacheMock = new Mock<IMemoryCache>();
        _redisMock = new Mock<IDatabase>();
        _connectionMock = new Mock<IConnectionMultiplexer>();
        
        _connectionMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                      .Returns(_redisMock.Object);
        
        // Note: In a real test, you might need to mock ConnectionMultiplexer.Connect
        // For now, creating service with mocked dependencies
        _productService = new ProductService(_repositoryMock.Object, _cacheMock.Object, _redisMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldCallRepositoryAddProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Product", Price = 10.99m, Stock = 5 };
        _repositoryMock.Setup(r => r.AddProductAsync(product)).ReturnsAsync(product);

        // Act
        var result = await _productService.AddAsync(product);

        // Assert
        Assert.Equal(product, result);
        _repositoryMock.Verify(r => r.AddProductAsync(product), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Product 1", Price = 10.99m, Stock = 5 },
            new() { Id = 2, Name = "Product 2", Price = 20.99m, Stock = 10 }
        };
        _repositoryMock.Setup(r => r.GetProductsAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllAsync();

        // Assert
        Assert.Equal(products, result);
        _repositoryMock.Verify(r => r.GetProductsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WhenProductNotInCache_ShouldFetchFromRepository()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Test Product", Price = 10.99m, Stock = 5 };
        
        _redisMock.Setup(r => r.StringGetAsync($"Product:{productId}", It.IsAny<CommandFlags>()))
                  .ReturnsAsync(RedisValue.Null);
        _repositoryMock.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);
        _redisMock.Setup(r => r.StringSetAsync(
            $"Product:{productId}",
            JsonSerializer.Serialize(product, new JsonSerializerOptions()),
            TimeSpan.FromDays(1),
            When.Always,
            CommandFlags.None
        )).ReturnsAsync(true);

        // Act
        var result = await _productService.GetAsync(productId);

        // Assert
        Assert.Equal(product, result);
        _repositoryMock.Verify(r => r.GetProductByIdAsync(productId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProductAndClearCache()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Updated Product", Price = 15.99m, Stock = 8 };
        _repositoryMock.Setup(r => r.UpdateProductAsync(product)).ReturnsAsync(product);
        _redisMock.Setup(r => r.KeyDeleteAsync($"Product:{product.Id}", It.IsAny<CommandFlags>()))
                  .ReturnsAsync(true);

        // Act
        var result = await _productService.UpdateAsync(product);

        // Assert
        Assert.Equal(product, result);
        _repositoryMock.Verify(r => r.UpdateProductAsync(product), Times.Once);
        _redisMock.Verify(r => r.KeyDeleteAsync($"Product:{product.Id}", It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldDeleteProductAndClearCache()
    {
        // Arrange
        var productId = 1;
        _repositoryMock.Setup(r => r.DeleteProductAsync(productId)).Returns(Task.CompletedTask);
        _redisMock.Setup(r => r.KeyDeleteAsync($"Product:{productId}", It.IsAny<CommandFlags>()))
                  .ReturnsAsync(true);

        // Act
        await _productService.Delete(productId);

        // Assert
        _repositoryMock.Verify(r => r.DeleteProductAsync(productId), Times.Once);
        _redisMock.Verify(r => r.KeyDeleteAsync($"Product:{productId}", It.IsAny<CommandFlags>()), Times.Once);
    }
} 