namespace User.Domain.Models.Request;

public class LogInRequest
{
	public string Email { get; set; } = default!;
	public string Password { get; set; } = default!;
}
