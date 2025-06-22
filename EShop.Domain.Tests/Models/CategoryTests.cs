using EShop.Domain.Models;
using Xunit;

namespace EShop.Domain.Tests.Models;

public class CategoryTests
{
    [Fact]
    public void Category_DefaultValues_AreSet()
    {
        var category = new Category();
        Assert.Equal(0, category.Id);
        Assert.Equal(string.Empty, category.Name);
    }

    [Fact]
    public void Category_SetName_Works()
    {
        var category = new Category { Name = "Test" };
        Assert.Equal("Test", category.Name);
    }
}