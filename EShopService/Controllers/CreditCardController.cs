using EShop.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using EShop.Application.Service;
using EShop.Domain.Exceptions.CreditCard;
using System.Net;
namespace EShop.Domain.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditCardController : ControllerBase
{
	protected ICreditCardService _creditCardService;

	public CreditCardController(ICreditCardService creditCardService)
	{
		_creditCardService = creditCardService;
	}
	[HttpGet]
	public IActionResult Get(string cardnumber)
	{
		try
		{
			_creditCardService.ValidateCard(cardnumber);
			return Ok(new { cardProvider = _creditCardService.GetCardType(cardnumber)});
		}
		catch (CardNumberTooLongException ex)
		{
			return StatusCode((int)HttpStatusCode.RequestUriTooLong, new { error = "The card number is too long", code = (int)HttpStatusCode.RequestUriTooLong });
		}
		catch (CardNumberTooShortException)
		{
			return BadRequest( new { error = "The card number is too short", code = (int)HttpStatusCode.BadRequest });
		}
		catch (CardNumberInvalidException)
		{
			return BadRequest(new { error = "Invalid Card Number", code = (int)HttpStatusCode.BadRequest });
		}
	}
}
