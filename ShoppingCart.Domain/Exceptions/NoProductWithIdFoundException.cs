using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Exceptions;

[Serializable]
public class NoProductWithIdFoundException : Exception
{
    public NoProductWithIdFoundException() : base("Can not add product to shopping cart becouse the product id is invalid")
    { }

    public NoProductWithIdFoundException(Exception innerException) : base("Can not add product to shopping cart becouse the product id is invalid", innerException)
    { }
}
