namespace User.Domain.Models.Request;

public class LogInRequest
{
	public string Username { get; set; } = default!;
	public string Password { get; set; } = default!;
}
