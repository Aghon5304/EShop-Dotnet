using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models.Response;

public class UserDeleteDTO
{
    public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
}
