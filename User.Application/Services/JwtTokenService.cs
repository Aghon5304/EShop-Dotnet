using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using User.Domain.Exceptions;
using User.Domain.Models.JWT;

namespace User.Application.Services;
public class JwtTokenService(IOptions<JwtSettings> settings) : IJwtTokenService
{
	private readonly JwtSettings _settings = settings.Value;

    public string GenerateToken(int userId, List<string> roles)
	{
		var rsa = RSA.Create();
		rsa.ImportFromPem(File.ReadAllText("/app/data/private.key")); 

        var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, userId.ToString())
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