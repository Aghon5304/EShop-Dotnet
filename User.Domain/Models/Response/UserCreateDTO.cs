﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models.Response;

public class UserCreateDTO
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
