using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Entities;

namespace User.Domain.Models.Response;

public class UserLoginDTO
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public ICollection<Role> Roles { get; set; }
}
