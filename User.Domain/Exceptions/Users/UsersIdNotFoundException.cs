using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions.User;

public class UserIdNotFoundException: Exception
{
    public UserIdNotFoundException() : base("User with given id does not exist")
    {}
    public UserIdNotFoundException(Exception innerException) : base("User with given id does not exist", innerException)
    {}
}
