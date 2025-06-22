using EShop.Domain.Models;
using Xunit;

namespace EShop.Domain.Tests.Models;

public class ProductTests
{
    [Fact]
    public void Product_DefaultValues_AreSet()
    {
        var product = new Product();
        Assert.Equal(0, product.Id);
        Assert.Equal(string.Empty, product.Name);
        Assert.Equal(string.Empty, product.Ean);
        Assert.Equal(0, product.Stock);
        Assert.Equal(string.Empty, product.Sku);
        Assert.Equal(0m, product.Price);
        Assert.Null(product.Category);
    }

    [Fact]
    public void Product_SetProperties_Works()
    {
        var category = new Category { Id = 1, Name = "TestCat" };
        var product = new Product
        {
            Name = "Test",
            Ean = "1234567890123",
            Price = 10.5m,
            Stock = 5,
            Sku = "SKU123",
            Category = category
        };
        Assert.Equal("Test", product.Name);
        Assert.Equal("1234567890123", product.Ean);
        Assert.Equal(10.5m, product.Price);
        Assert.Equal(5, product.Stock);
        Assert.Equal("SKU123", product.Sku);
        Assert.Equal(category, product.Category);
    }
}