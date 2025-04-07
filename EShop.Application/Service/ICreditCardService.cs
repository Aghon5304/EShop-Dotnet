using EShop.Domain.Exceptions.CreditCard;
using System.Text.RegularExpressions;

namespace EShop.Application.Service
{
	public interface ICreditCardService
	{
		public  bool ValidateCard(string cardNumber);
		public string GetCardType(string cardNumber);
	}
}
