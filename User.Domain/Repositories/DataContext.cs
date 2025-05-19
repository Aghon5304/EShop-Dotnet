using Microsoft.EntityFrameworkCore;
using User.Domain.Models;
namespace User.Domain.Repositories;
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Users> Users { get; set; } = null!;
    public DbSet<Role> Role { get; set; } = null!;
}