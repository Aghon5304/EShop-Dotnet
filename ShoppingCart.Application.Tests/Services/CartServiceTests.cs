using Moq;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Models;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Application.Tests.Services;

public class CartServiceTests
{
    private readonly Mock<ICartRepository> _repositoryMock;
    private readonly CartService _cartService;

    public CartServiceTests()
    {
        _repositoryMock = new Mock<ICartRepository>();
        _cartService = new CartService(_repositoryMock.Object);
    }

    [Fact]
    public void AddProductToCart_WithExistingCart_ShouldAddProductToCart()
    {
        // Arrange
        var cartId = 1;
        var product = new Product { Id = 1 };
        var existingCart = new Cart 
        { 
            Id = cartId, 
            Products = new List<Product>() 
        };

        _repositoryMock.Setup(r => r.FindById(cartId))
                      .Returns(existingCart);

        // Act
        _cartService.AddProductToCart(cartId, product);

        // Assert
        Assert.Contains(product, existingCart.Products);
        _repositoryMock.Verify(r => r.FindById(cartId), Times.Once);
        _repositoryMock.Verify(r => r.Add(existingCart), Times.Once);
    }

    [Fact]
    public void AddProductToCart_WithNewCart_ShouldCreateNewCartAndAddProduct()
    {
        // Arrange
        var cartId = 1;
        var product = new Product { Id = 1 };

        _repositoryMock.Setup(r => r.FindById(cartId))
                      .Returns((Cart)null);

        // Act
        _cartService.AddProductToCart(cartId, product);

        // Assert
        _repositoryMock.Verify(r => r.FindById(cartId), Times.Once);
        _repositoryMock.Verify(r => r.Add(It.Is<Cart>(c => 
            c.Id == cartId && 
            c.Products.Contains(product))), Times.Once);
    }

    [Fact]
    public void RemoveProductFromCart_WithExistingProduct_ShouldRemoveProduct()
    {
        // Arrange
        var cartId = 1;
        var productId = 1;
        var product = new Product { Id = productId };
        var cart = new Cart 
        { 
            Id = cartId, 
            Products = new List<Product> { product } 
        };

        _repositoryMock.Setup(r => r.FindById(cartId))
                      .Returns(cart);

        // Act
        _cartService.RemoveProductFromCart(cartId, productId);

        // Assert
        Assert.DoesNotContain(product, cart.Products);
        _repositoryMock.Verify(r => r.FindById(cartId), Times.Once);
        _repositoryMock.Verify(r => r.Update(cart), Times.Once);
    }

    [Fact]
    public void RemoveProductFromCart_WithNonExistentCart_ShouldNotThrow()
    {
        // Arrange
        var cartId = 1;
        var productId = 1;

        _repositoryMock.Setup(r => r.FindById(cartId))
                      .Returns((Cart)null);

        // Act & Assert
        _cartService.RemoveProductFromCart(cartId, productId);
        _repositoryMock.Verify(r => r.FindById(cartId), Times.Once);
        _repositoryMock.Verify(r => r.Update(It.IsAny<Cart>()), Times.Never);
    }

    [Fact]
    public void RemoveProductFromCart_WithNonExistentProduct_ShouldNotRemoveAnything()
    {
        // Arrange
        var cartId = 1;
        var productId = 999;
        var existingProduct = new Product { Id = 1 };
        var cart = new Cart 
        { 
            Id = cartId, 
            Products = new List<Product> { existingProduct } 
        };

        _repositoryMock.Setup(r => r.FindById(cartId))
                      .Returns(cart);

        // Act
        _cartService.RemoveProductFromCart(cartId, productId);

        // Assert
        Assert.Contains(existingProduct, cart.Products);
        Assert.Single(cart.Products);
        _repositoryMock.Verify(r => r.Update(It.IsAny<Cart>()), Times.Never);
    }

    [Fact]
    public void GetCart_WithExistingCart_ShouldReturnCart()
    {
        // Arrange
        var cartId = 1;
        var product = new Product { Id = 1 };
        var cart = new Cart 
        { 
            Id = cartId, 
            Products = new List<Product> { product } 
        };

        _repositoryMock.Setup(r => r.FindById(cartId))
                      .Returns(cart);

        // Act
        var result = _cartService.GetCart(cartId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cartId, result.Id);
        Assert.Single(result.Products);
        Assert.Equal(product.Id, result.Products.First().Id);
        _repositoryMock.Verify(r => r.FindById(cartId), Times.Once);
    }

    [Fact]
    public void GetCart_WithNonExistentCart_ShouldReturnNull()
    {
        // Arrange
        var cartId = 1;

        _repositoryMock.Setup(r => r.FindById(cartId))
                      .Returns((Cart)null);

        // Act
        var result = _cartService.GetCart(cartId);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.FindById(cartId), Times.Once);
    }

    [Fact]
    public void GetAllCarts_ShouldReturnAllCarts()
    {
        // Arrange
        var carts = new List<Cart>
        {
            new() { Id = 1, Products = new List<Product> { new() { Id = 1 } } },
            new() { Id = 2, Products = new List<Product> { new() { Id = 2 } } }
        };

        _repositoryMock.Setup(r => r.GetAll())
                      .Returns(carts);

        // Act
        var result = _cartService.GetAllCarts();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result.First().Id);
        Assert.Equal(2, result.Last().Id);
        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void GetAllCarts_WithEmptyRepository_ShouldReturnEmptyList()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAll())
                      .Returns(new List<Cart>());

        // Act
        var result = _cartService.GetAllCarts();

        // Assert
        Assert.Empty(result);
        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public void AddProductToCart_WithInvalidCartId_ShouldStillProcess(int invalidCartId)
    {
        // Arrange
        var product = new Product { Id = 1 };
        _repositoryMock.Setup(r => r.FindById(invalidCartId))
                      .Returns((Cart)null);

        // Act
        _cartService.AddProductToCart(invalidCartId, product);

        // Assert
        _repositoryMock.Verify(r => r.Add(It.Is<Cart>(c => c.Id == invalidCartId)), Times.Once);
    }
} 