using EShopService.Controllers;
using EShop.Application.Service;
using EShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EShopService.Tests.Controllers;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly CategoryController _categoryController;

    public CategoryControllerTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _categoryController = new CategoryController(_categoryServiceMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Electronics" },
            new() { Id = 2, Name = "Clothing" }
        };
        _categoryServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

        // Act
        var result = await _categoryController.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(categories, okResult.Value);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnCategory()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId, Name = "Electronics" };
        _categoryServiceMock.Setup(s => s.GetAsync(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _categoryController.Get(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(category, okResult.Value);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var categoryId = 999;
        _categoryServiceMock.Setup(s => s.GetAsync(categoryId)).ReturnsAsync((Category)null);

        // Act
        var result = await _categoryController.Get(categoryId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Post_WithValidName_ShouldCreateCategory()
    {
        // Arrange
        var categoryName = "New Category";
        var createdCategory = new Category { Id = 1, Name = categoryName };
        _categoryServiceMock.Setup(s => s.AddAsync(categoryName)).ReturnsAsync(createdCategory);

        // Act
        var result = await _categoryController.Post(categoryName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(createdCategory, okResult.Value);
        _categoryServiceMock.Verify(s => s.AddAsync(categoryName), Times.Once);
    }

    [Fact]
    public async Task Put_WithValidCategory_ShouldUpdateCategory()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId, Name = "Updated Category" };
        _categoryServiceMock.Setup(s => s.Update(category)).ReturnsAsync(category);

        // Act
        var result = await _categoryController.Put(categoryId, category);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(category, okResult.Value);
        _categoryServiceMock.Verify(s => s.Update(category), Times.Once);
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldSoftDeleteCategory()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId, Name = "To Delete", Deleted = false };
        var updatedCategory = new Category { Id = categoryId, Name = "To Delete", Deleted = true };
        
        _categoryServiceMock.Setup(s => s.GetAsync(categoryId)).ReturnsAsync(category);
        _categoryServiceMock.Setup(s => s.Update(It.IsAny<Category>())).ReturnsAsync(updatedCategory);

        // Act
        var result = await _categoryController.Delete(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultCategory = Assert.IsType<Category>(okResult.Value);
        Assert.True(resultCategory.Deleted);
        _categoryServiceMock.Verify(s => s.GetAsync(categoryId), Times.Once);
        _categoryServiceMock.Verify(s => s.Update(It.Is<Category>(c => c.Deleted == true)), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Post_WithInvalidName_ShouldStillProcessRequest(string invalidName)
    {
        // Arrange
        var createdCategory = new Category { Name = invalidName };
        _categoryServiceMock.Setup(s => s.AddAsync(invalidName)).ReturnsAsync(createdCategory);

        // Act
        var result = await _categoryController.Post(invalidName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        _categoryServiceMock.Verify(s => s.AddAsync(invalidName), Times.Once);
    }

    [Fact]
    public async Task Put_WithMismatchedId_ShouldStillUpdateCategory()
    {
        // Arrange
        var urlId = 1;
        var category = new Category { Id = 2, Name = "Category" };
        _categoryServiceMock.Setup(s => s.Update(category)).ReturnsAsync(category);

        // Act
        var result = await _categoryController.Put(urlId, category);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        _categoryServiceMock.Verify(s => s.Update(category), Times.Once);
    }
} 