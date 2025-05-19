using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using User.Application.Services;
using User.Domain.Models.Request;
namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController(ILoginService loginService) : ControllerBase
{
    protected ILoginService _loginService = loginService;

    [HttpPost]
    public IActionResult Login([FromBody] LogInRequest request)
    {
        try
        {
            var token = _loginService.Login(request.Username, request.Password);
            return Ok(new { token });
        }
        catch(InvalidCredentialException)
        {
            return Unauthorized();
        }
    }
    [HttpGet]
    [Authorize]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult AdminPage()
    {
        return Ok();
    }
}
