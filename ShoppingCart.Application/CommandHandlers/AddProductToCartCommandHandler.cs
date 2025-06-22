using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;
using MediatR;
using ShoppingCart.Domain.Command;
using ShoppingCart.Domain.Exceptions;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Application.CommandHandlers;

public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand>

{
    private readonly ICartAdder _cartAdder;
    private readonly HttpClient _httpClient;

    public AddProductToCartCommandHandler(ICartAdder cartAdder, HttpClient httpClient)
    {
        _cartAdder = cartAdder;
        _httpClient = httpClient;
    }

    public Task Handle(AddProductToCartCommand command, CancellationToken cancellationToken)
    {
        _cartAdder.AddProductToCart(command.CartId, new Product { Id = command.ProductId, Name = command.ProductName, Price = command.ProductPrice });
        return Task.CompletedTask;
    }
}
