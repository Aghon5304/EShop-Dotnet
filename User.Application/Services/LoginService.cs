using User.Application.Services;
using User.Domain.Exceptions;
using User.Domain.Exceptions.Login;
namespace User.Application.Services
{
	public class LoginService(IJwtTokenService jwtTokenService) : ILoginService
	{
		protected IJwtTokenService _jwtTokenService = jwtTokenService;

        public string Login(string username, string password)
		{
			if (username == "admin" && password == "password")
			{
				var roles = new List<string> { "Client", "Employee", "Administrator" };
				var token = _jwtTokenService.GenerateToken(123, roles);
				return token;
			}
			else
			{
				throw new InvalidCredentialsException();
			}

		}
	}
}