namespace User.Application.Service.Service;
public interface IJwtTokenService
{
	string GenerateToken(int userId, List<string> roles);
}