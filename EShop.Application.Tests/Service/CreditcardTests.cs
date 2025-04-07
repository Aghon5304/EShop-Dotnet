using EShop.Application.Service;
using EShop.Domain.Exceptions.CreditCard;

namespace EShop.Application.Tests.service;
public class CreditCardServiceTest
{
	[Fact]
	public void Creditcard_tooShort_returnException()
	{
		var creditCardService = new CreditcardService();
		Assert.Throws<CardNumberTooShortException>(() => creditCardService.ValidateCard("123456789"));
	}
	[Theory]
	[InlineData("4111111111111111", true)]
	[InlineData("371449635398431", true)]
	[InlineData("1234567891123456783", true)]
	[InlineData("1234567891123456789123", false)]
	public void Creditcard_length_CorrectOrTooLong(string cardNumber, bool result)
	{
		var creditCardService = new CreditcardService();
		if (result)
		{
			Assert.True( creditCardService.ValidateCard(cardNumber));
		}
		else
		{
			Assert.Throws<CardNumberTooLongException>(() => creditCardService.ValidateCard(cardNumber));
		}
	}

	[Theory]
	[InlineData("3497-7965-8312-797", true)]
	[InlineData("3497 7965 8312 797", true)]
	[InlineData("349779658312797", true)]
	[InlineData("3497/7965/8312/797", false)]
	[InlineData("3497:7965:8312:797", false)]
	public void Creditcard_symbols_correct(string cardNumber, bool result)
	{
		var creditCardService = new CreditcardService();
		if (result)
		{
			Assert.True(condition: creditCardService.ValidateCard(cardNumber));
		}
		else
		{
			Assert.Throws<CardNumberInvalidException>(() => creditCardService.ValidateCard(cardNumber));
		}
	}

	[Theory]
	[InlineData("3497 7965 8312 797",true)]
	[InlineData("345-470-784-783-010",true)]
	[InlineData("378523393817437", true)]
	[InlineData("4024-0071-6540-1778", true)]
	[InlineData("4532 2080 2150 4434", true)]
	[InlineData("4532289052809181", true)]
	[InlineData("5530016454538418", true)]
	[InlineData("5551561443896215", true)]
	[InlineData("5131208517986691", true)]
	public void Creditcard_LuhnaAlgorithm_correct(string cardNumber, bool result)
	{
		var creditCardService = new CreditcardService();
		if (result)
		{
			Assert.True(creditCardService.ValidateCard(cardNumber));
		}
		else
		{
			Assert.False(creditCardService.ValidateCard(cardNumber));
		}
	}

	[Theory]
	[InlineData("American Express", "3497 7965 8312 797")]
	[InlineData("American Express", "345-470-784-783-010")]
	[InlineData("American Express", "378523393817437")]
	[InlineData("Visa", "4024-0071-6540-1778")]
	[InlineData("Visa", "4532 2080 2150 4434")]
	[InlineData("Visa", "4532289052809181")]
	[InlineData("MasterCard", "5530016454538418")]
	[InlineData("MasterCard", "5551561443896215")]
	[InlineData("MasterCard", "5131208517986691")]
	public void Creditcard_GetCorrectCardType_correct(string cardType, string cardNumber)
	{

		var creditCardService = new CreditcardService();
		Assert.Equal(cardType, creditCardService.GetCardType(cardNumber));
	}
}