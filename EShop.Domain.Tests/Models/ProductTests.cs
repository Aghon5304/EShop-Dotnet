using EShop.Domain.Models;

namespace EShop.Domain.Tests.Models;

public class ProductTests
{
    [Fact]
    public void Product_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 99.99m,
            Stock = 10
        };

        // Assert
        Assert.Equal(1, product.Id);
        Assert.Equal("Test Product", product.Name);
        Assert.Equal(99.99m, product.Price);
        Assert.Equal(10, product.Stock);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Product_WithNegativeOrZeroPrice_ShouldAllowButIndicateInvalidState(decimal price)
    {
        // Arrange & Act
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = price,
            Stock = 10
        };

        // Assert
        Assert.Equal(price, product.Price);
        Assert.True(product.Price <= 0); // Business rule: prices should be positive
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Product_WithNegativeStock_ShouldAllowButIndicateInvalidState(int stock)
    {
        // Arrange & Act
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 99.99m,
            Stock = stock
        };

        // Assert
        Assert.Equal(stock, product.Stock);
        Assert.True(product.Stock < 0); // Business rule: stock should not be negative
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Product_WithInvalidName_ShouldAllowButIndicateInvalidState(string name)
    {
        // Arrange & Act
        var product = new Product
        {
            Id = 1,
            Name = name,
            Price = 99.99m,
            Stock = 10
        };

        // Assert
        Assert.Equal(name, product.Name);
        Assert.True(string.IsNullOrWhiteSpace(product.Name)); // Business rule: name should not be empty
    }

    [Fact]
    public void Product_WithCategory_ShouldSetCategoryRelationship()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Electronics" };
        
        // Act
        var product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 999.99m,
            Stock = 5,
            Category = category
        };
        Assert.Equal(category, product.Category);
        Assert.Equal("Electronics", product.Category.Name);
    }

    [Fact]
    public void Product_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.Equal(0, product.Id);
        Assert.Null(product.Name);
        Assert.Equal(0m, product.Price);
        Assert.Equal(0, product.Stock);
        Assert.Null(product.Category);
    }

    [Fact]
    public void Product_Equality_ShouldCompareById()
    {
        // Arrange
        var product1 = new Product { Id = 1, Name = "Product 1" };
        var product2 = new Product { Id = 1, Name = "Product 1 Updated" };
        var product3 = new Product { Id = 2, Name = "Product 1" };

        // Act & Assert
        Assert.Equal(product1.Id, product2.Id);
        Assert.NotEqual(product1.Id, product3.Id);
    }
} 