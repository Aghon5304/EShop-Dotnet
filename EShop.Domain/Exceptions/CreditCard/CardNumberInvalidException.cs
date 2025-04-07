using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Exceptions.CreditCard;

[Serializable]
public class CardNumberInvalidException : Exception
{
	public CardNumberInvalidException() : base("CreditCard is invalid")
	{ }

	public CardNumberInvalidException(Exception innerException) : base("CreditCard is invalid", innerException)
	{ }
}
