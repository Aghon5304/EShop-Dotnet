using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;

namespace User.Domain.Repositories;
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User.Domain.Models.Entities.User> User { get; set; } = null!;
    public DbSet<Role> Role { get; set; } = null!;
}