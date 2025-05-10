using EShop.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using EShop.Application.Service;
using EShop.Domain.Exceptions.CreditCard;
using System.Net;
namespace EShopService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditCardController(ICreditCardService creditCardService) : ControllerBase
{
	protected ICreditCardService _creditCardService = creditCardService;

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
			return StatusCode((int)HttpStatusCode.RequestUriTooLong, new { error = ex.Message, code = (int)HttpStatusCode.RequestUriTooLong });
		}
		catch (CardNumberTooShortException ex)
		{
			return BadRequest( new { error = ex.Message, code = (int)HttpStatusCode.BadRequest });
		}
		catch (CardNumberInvalidException ex)
		{
			return BadRequest(new { error = ex.Message, code = (int)HttpStatusCode.BadRequest });
		}
	}
}
