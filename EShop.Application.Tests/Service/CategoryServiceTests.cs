using EShop.Application.Service;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using StackExchange.Redis;
using System.Text.Json;

namespace EShop.Application.Tests.Service;

public class CategoryServiceTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<IDatabase> _redisMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _cacheMock = new Mock<IMemoryCache>();
        _redisMock = new Mock<IDatabase>();
        
        // Note: In a real test environment, you would mock ConnectionMultiplexer properly
        _categoryService = new CategoryService(_repositoryMock.Object, _cacheMock.Object, _redisMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldCreateAndAddCategory()
    {
        // Arrange
        var categoryName = "Electronics";
        var expectedCategory = new Category { Name = categoryName };
        _repositoryMock.Setup(r => r.AddCategoryAsync(It.IsAny<Category>()))
                      .ReturnsAsync(expectedCategory);

        // Act
        var result = await _categoryService.AddAsync(categoryName);

        // Assert
        Assert.Equal(categoryName, result.Name);
        _repositoryMock.Verify(r => r.AddCategoryAsync(It.Is<Category>(c => c.Name == categoryName)), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Electronics" },
            new() { Id = 2, Name = "Clothing" }
        };
        _repositoryMock.Setup(r => r.GetCategoryAsync()).ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetAllAsync();

        // Assert
        Assert.Equal(categories, result);
        _repositoryMock.Verify(r => r.GetCategoryAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WhenCategoryNotInCache_ShouldFetchFromRepository()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId, Name = "Electronics" };
        
        _redisMock.Setup(r => r.StringGetAsync($"category:{categoryId}", It.IsAny<CommandFlags>()))
                  .ReturnsAsync(RedisValue.Null);
        _repositoryMock.Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);
        _redisMock.Setup(r => r.StringSetAsync($"category:{categoryId}",
                                               JsonSerializer.Serialize(category, new JsonSerializerOptions()), 
                                               TimeSpan.FromDays(1), 
                                               It.IsAny<When>(), 
                                               It.IsAny<CommandFlags>()))
                  .ReturnsAsync(true);

        // Act
        var result = await _categoryService.GetAsync(categoryId);

        // Assert
        Assert.Equal(category, result);
        _repositoryMock.Verify(r => r.GetCategoryByIdAsync(categoryId), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WhenCategoryInCache_ShouldReturnCachedCategory()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId, Name = "Electronics" };
        var cachedJson = JsonSerializer.Serialize(category);
        
        _redisMock.Setup(r => r.StringGetAsync($"category:{categoryId}", It.IsAny<CommandFlags>()))
                  .ReturnsAsync(cachedJson);

        // Act
        var result = await _categoryService.GetAsync(categoryId);

        // Assert
        Assert.Equal(category.Name, result.Name);
        _repositoryMock.Verify(r => r.GetCategoryByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Update_ShouldUpdateCategoryAndClearCache()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Updated Electronics" };
        _repositoryMock.Setup(r => r.UpdateCategoryAsync(category)).ReturnsAsync(category);
        _redisMock.Setup(r => r.KeyDeleteAsync("category:all", It.IsAny<CommandFlags>()))
                  .ReturnsAsync(true);

        // Act
        var result = await _categoryService.Update(category);

        // Assert
        Assert.Equal(category, result);
        _repositoryMock.Verify(r => r.UpdateCategoryAsync(category), Times.Once);
        _redisMock.Verify(r => r.KeyDeleteAsync("category:all", It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldDeleteCategoryAndClearCache()
    {
        // Arrange
        var categoryId = 1;
        _repositoryMock.Setup(r => r.DeleteCategoryAsync(categoryId)).Returns(Task.CompletedTask);
        _redisMock.Setup(r => r.KeyDeleteAsync($"category:{categoryId}", It.IsAny<CommandFlags>()))
                  .ReturnsAsync(true);

        // Act
        await _categoryService.Delete(categoryId);

        // Assert
        _repositoryMock.Verify(r => r.DeleteCategoryAsync(categoryId), Times.Once);
        _redisMock.Verify(r => r.KeyDeleteAsync($"category:{categoryId}", It.IsAny<CommandFlags>()), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task AddAsync_WithInvalidName_ShouldStillCreateCategory(string invalidName)
    {
        // Arrange
        var expectedCategory = new Category { Name = invalidName };
        _repositoryMock.Setup(r => r.AddCategoryAsync(It.IsAny<Category>()))
                      .ReturnsAsync(expectedCategory);

        // Act
        var result = await _categoryService.AddAsync(invalidName);

        // Assert
        Assert.Equal(invalidName, result.Name);
    }
} 