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
public class CategoryServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllCategories()
    {
        // Arrange
        var mockRepo = new Mock<IRepository>();
        var mockCache = new Mock<IMemoryCache>();
        var mockRedisDb = new Mock<IDatabase>();

        var categories = new List<Category> { new Category { Id = 1, Name = "Test" } };
        mockRepo.Setup(r => r.GetCategoryAsync()).ReturnsAsync(categories);

        var service = new CategoryService(mockRepo.Object, mockCache.Object, mockRedisDb.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Test", result[0].Name);
    }

    [Fact]
    public async Task AddAsync_CallsRepositoryAndReturnsCategory()
    {
        // Arrange
        var mockRepo = new Mock<IRepository>();
        var mockCache = new Mock<IMemoryCache>();
        var mockRedisDb = new Mock<IDatabase>();

        var category = new Category { Id = 1, Name = "Cat" };
        mockRepo.Setup(r => r.AddCategoryAsync(It.IsAny<Category>())).ReturnsAsync(category);

        var service = new CategoryService(mockRepo.Object, mockCache.Object, mockRedisDb.Object);

        // Act
        var result = await service.AddAsync("Test");

        // Assert
        Assert.Equal(category, result);
    }
}