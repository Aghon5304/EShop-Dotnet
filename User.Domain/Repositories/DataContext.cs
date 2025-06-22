using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;

namespace User.Domain.Repositories;
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Models.Entities.User> User { get; set; }
    public DbSet<Role> Role { get; set; }
}