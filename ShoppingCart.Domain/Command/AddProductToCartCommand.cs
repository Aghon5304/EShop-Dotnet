using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ShoppingCart.Domain.Command;

public class AddProductToCartCommand:IRequest
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
}
