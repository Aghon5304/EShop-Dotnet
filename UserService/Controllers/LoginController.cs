using Microsoft.AspNetCore.Mvc;
using User.Domain.Models.Request;
namespace UserService.Controllers;

public class LoginController : ControllerBase
{
	public class LoginService();
	[HttpPost]
	public IActionResult Login([FromBody] LogInRequest request)
	{
		if (request.Username == "admin" && request.Password == "admin")
		{
			return Ok("Zalogowano pomyślnie");
		}
		else
		{
			return Unauthorized("Niepoprawne dane logowania");
		}
	}
}
