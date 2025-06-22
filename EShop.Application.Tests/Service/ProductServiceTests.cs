using System.Collections.Generic;
using System.Threading.Tasks;
using EShop.Application.Service;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using StackExchange.Redis;
using Xunit;


namespace EShop.Application.Tests.service;
public class ProductServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts()
    {
        // Arrange
        var mockRepo = new Mock<IRepository>();
        var mockCache = new Mock<IMemoryCache>();
        var mockRedisDb = new Mock<IDatabase>();

        var products = new List<Product> { new Product { Id = 1, Name = "Test" } };
        mockRepo.Setup(r => r.GetProductsAsync()).ReturnsAsync(products);

        var service = new ProductService(mockRepo.Object, mockCache.Object, mockRedisDb.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Test", result[0].Name);
    }

    [Fact]
    public async Task AddAsync_CallsRepositoryAndReturnsProduct()
    {
        // Arrange
        var mockRepo = new Mock<IRepository>();
        var mockCache = new Mock<IMemoryCache>();
        var mockRedisDb = new Mock<IDatabase>();
        var product = new Product { Id = 1, Name = "Test" };

        mockRepo.Setup(r => r.AddProductAsync(product)).ReturnsAsync(product);

        var service = new ProductService(mockRepo.Object, mockCache.Object, mockRedisDb.Object);

        // Act
        var result = await service.AddAsync(product);

        // Assert
        Assert.Equal(product, result);
    }
}