using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ShoppingCart.Domain.Command;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Domain.ViewModels;

public class MappingProfiles: Profile
{
    MappingProfiles() 
    { 
    }
}
