using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ShoppingCart.Domain.Command;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.CommandHandlers;

public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand>
{
    private readonly ICartRemover _cartRemover;

    public RemoveProductFromCartCommandHandler(ICartRemover cartRemover)
    {
        _cartRemover = cartRemover;
    }

    public Task Handle(RemoveProductFromCartCommand command, CancellationToken cancellationToken)
    {
        _cartRemover.RemoveProductFromCart(command.CartId, command.ProductId);
        return Task.CompletedTask;
    }
}
