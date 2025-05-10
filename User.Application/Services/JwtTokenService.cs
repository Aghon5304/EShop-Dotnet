using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using User.Domain.Exceptions;

namespace User.Application.Service.Service;
public class JwtTokenService : IJwtTokenService
{
	private readonly JwtSettings _settings;

	public JwtTokenService(IOptions<JwtSettings> settings)
	{
		_settings = settings.Value;
	}
	public string GenerateToken(int userId, List<string> roles)
	{
		var rsa = RSA.Create();
		rsa.ImportFromPem(File.ReadAllText("..docker/data/private.key")); // Załaduj klucz prywatny RSA
		var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, userId.ToString())
		};

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var token = new JwtSecurityToken(
			issuer: _settings.Issuer,
			audience: _settings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}