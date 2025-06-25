namespace User.Application.Services;

public interface ILoginService
{
	Task<string> Login(string username, string password);
}