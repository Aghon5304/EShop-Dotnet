using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Exceptions.CreditCard;

[Serializable]
public class CardNumberTooLongException : Exception
{
	public CardNumberTooLongException() : base("CreditCard too long")
	{ }

	public CardNumberTooLongException(Exception innerException) : base("CreditCard too long", innerException)
	{ }
}
