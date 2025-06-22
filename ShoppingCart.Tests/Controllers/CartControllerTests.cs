using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingCart.Controllers;
using ShoppingCart.Domain.Command;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Queries;

namespace ShoppingCart.Tests.Controllers;

public class CartControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CartController _cartController;

    public CartControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _cartController = new CartController(_mediatorMock.Object);
    }

    [Fact]
    public async Task AddProductToCart_WithValidCommand_ShouldReturnOk()
    {
        // Arrange
        var command = new AddProductToCartCommand
        {
            CartId = 1,
            Product = new Product { Id = 1 }
        };

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

        // Act
        var result = await _cartController.AddProductToCart(command);

        // Assert
        Assert.IsType<OkResult>(result);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveProductFromCart_WithValidCommand_ShouldReturnOk()
    {
        // Arrange
        var command = new RemoveProductFromCartCommand
        {
            CartId = 1,
            ProductId = 1
        };

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

        // Act
        var result = await _cartController.RemoveProductFromCart(command);

        // Assert
        Assert.IsType<OkResult>(result);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCart_WithExistingCart_ShouldReturnOkWithCart()
    {
        // Arrange
        var cartId = 1;
        var cart = new Cart 
        { 
            Id = cartId, 
            Products = new List<Product> { new() { Id = 1 } } 
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetCartQuery>(q => q.CartId == cartId), 
                                      It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cart);

        // Act
        var result = await _cartController.GetCart(cartId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(cart, okResult.Value);
        _mediatorMock.Verify(m => m.Send(It.Is<GetCartQuery>(q => q.CartId == cartId), 
                                       It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCart_WithNonExistentCart_ShouldReturnNotFound()
    {
        // Arrange
        var cartId = 999;

        _mediatorMock.Setup(m => m.Send(It.Is<GetCartQuery>(q => q.CartId == cartId), 
                                      It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Cart)null);

        // Act
        var result = await _cartController.GetCart(cartId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _mediatorMock.Verify(m => m.Send(It.Is<GetCartQuery>(q => q.CartId == cartId), 
                                       It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllCarts_ShouldReturnOkWithAllCarts()
    {
        // Arrange
        var carts = new List<Cart>
        {
            new() { Id = 1, Products = new List<Product> { new() { Id = 1 } } },
            new() { Id = 2, Products = new List<Product> { new() { Id = 2 } } }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCartsQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(carts);

        // Act
        var result = await _cartController.GetAllCarts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(carts, okResult.Value);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllCartsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllCarts_WithEmptyResult_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyCarts = new List<Cart>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCartsQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(emptyCarts);

        // Act
        var result = await _cartController.GetAllCarts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCarts = Assert.IsType<List<Cart>>(okResult.Value);
        Assert.Empty(returnedCarts);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public async Task GetCart_WithInvalidCartId_ShouldStillCallMediator(int invalidCartId)
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.Is<GetCartQuery>(q => q.CartId == invalidCartId), 
                                      It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Cart)null);

        // Act
        var result = await _cartController.GetCart(invalidCartId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _mediatorMock.Verify(m => m.Send(It.Is<GetCartQuery>(q => q.CartId == invalidCartId), 
                                       It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddProductToCart_WithNullProduct_ShouldStillCallMediator()
    {
        // Arrange
        var command = new AddProductToCartCommand
        {
            CartId = 1,
            Product = null
        };

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

        // Act
        var result = await _cartController.AddProductToCart(command);

        // Assert
        Assert.IsType<OkResult>(result);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public async Task RemoveProductFromCart_WithInvalidProductId_ShouldStillCallMediator(int invalidProductId)
    {
        // Arrange
        var command = new RemoveProductFromCartCommand
        {
            CartId = 1,
            ProductId = invalidProductId
        };

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

        // Act
        var result = await _cartController.RemoveProductFromCart(command);

        // Assert
        Assert.IsType<OkResult>(result);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }
} 