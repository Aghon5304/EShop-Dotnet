using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace User.Domain.Models.Entities;
public class User
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(100)]
	public string Username { get; set; }

	[Required]
	[MaxLength(255)]
	public string Email { get; set; }

	[Required]
	public string PasswordHash { get; set; }

	public ICollection<Role> Roles { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
