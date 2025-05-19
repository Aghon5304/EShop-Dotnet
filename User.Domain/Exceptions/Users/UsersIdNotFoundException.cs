using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions.Users;

public class UsersIdNotFoundException: Exception
{
    public UsersIdNotFoundException() : base("User with given id does not exist")
    {}
    public UsersIdNotFoundException(Exception innerException) : base("User with given id does not exist", innerException)
    {}
}
