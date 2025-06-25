using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models.Response
{
    public class UserUpdatePasswordDTO
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        
    }
}
