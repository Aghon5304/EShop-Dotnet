using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Entities;

namespace User.Domain.Models.Response;

public class UserUpdateLoginAtDTO
{
    public int Id { get; set; }
    public DateTime LastLoginAt { get; set; }
}
