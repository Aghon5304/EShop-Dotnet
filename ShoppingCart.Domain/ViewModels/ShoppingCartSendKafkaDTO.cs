using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.ViewModels;

public class ShoppingCartSendKafkaDTO
{
    public string Email { get; set; }
    public string Message { get; set; }
}
