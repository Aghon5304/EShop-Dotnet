using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using MediatR;
using ShoppingCart.Domain.Command;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.CommandHandlers
{
    public class ProcessShoppingCartCommandHandler : IRequestHandler<ProcessCartCommand>
    {
        private readonly ICartProcess _cartProcces;

        public ProcessShoppingCartCommandHandler(ICartProcess cartProcces)
        {
            _cartProcces = cartProcces;
        }
        public Task Handle(ProcessCartCommand request, CancellationToken cancellationToken)
        {
            _cartProcces.ProcessCart( request.CartId,request.email);
            return Task.CompletedTask;
        }
    }
}
