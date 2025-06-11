using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;

namespace ShoppingCart.Domain.Models;

public class Cart
{
    public int Id { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
}
