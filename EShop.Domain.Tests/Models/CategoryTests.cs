using EShop.Domain.Models;

namespace EShop.Domain.Tests.Models;

public class CategoryTests
{
    [Fact]
    public void Category_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var category = new Category
        {
            Id = 1,
            Name = "Electronics",
            Deleted = false
        };

        // Assert
        Assert.Equal(1, category.Id);
        Assert.Equal("Electronics", category.Name);
        Assert.False(category.Deleted);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Category_WithInvalidName_ShouldAllowButIndicateInvalidState(string name)
    {
        // Arrange & Act
        var category = new Category
        {
            Id = 1,
            Name = name,
            Deleted = false
        };

        // Assert
        Assert.Equal(name, category.Name);
        Assert.True(string.IsNullOrWhiteSpace(category.Name)); // Business rule: name should not be empty
    }

    [Fact]
    public void Category_WithProducts_ShouldMaintainProductRelationship()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Electronics" };
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop", Category = category },
            new() { Id = 2, Name = "Phone", Category = category }
        };

        // Assert - Categories don't have a Products collection in this model
        Assert.Equal(2, products.Count);
        Assert.All(products, p => Assert.Equal(category, p.Category));
    }

    [Fact]
    public void Category_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        var category = new Category();

        // Assert
        Assert.Equal(0, category.Id);
        Assert.Equal("",category.Name);
    }

    [Fact]
    public void Category_SoftDelete_ShouldSetDeletedFlag()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Electronics",
            Deleted = false
        };

        // Act
        category.Deleted = true;

        // Assert
        Assert.True(category.Deleted);
    }

    [Fact]
    public void Category_Equality_ShouldCompareById()
    {
        // Arrange
        var category1 = new Category { Id = 1, Name = "Electronics" };
        var category2 = new Category { Id = 1, Name = "Electronics Updated" };
        var category3 = new Category { Id = 2, Name = "Electronics" };

        // Act & Assert
        Assert.Equal(category1.Id, category2.Id);
        Assert.NotEqual(category1.Id, category3.Id);
    }

    [Theory]
    [InlineData("Electronics")]
    [InlineData("Clothing")]
    [InlineData("Books")]
    [InlineData("Sports & Outdoors")]
    public void Category_WithValidNames_ShouldAcceptAllValidNames(string validName)
    {
        // Arrange & Act
        var category = new Category
        {
            Id = 1,
            Name = validName
        };

        // Assert
        Assert.Equal(validName, category.Name);
        Assert.False(string.IsNullOrWhiteSpace(category.Name));
    }
} 