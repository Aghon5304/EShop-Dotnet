
using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;
using User.Domain.Repositories;

namespace User.Domain.Seeders
{
    public class UserSeeder(DataContext context) : IUserSeeder
    {
        public async Task Seed()
        {
            if (!context.Role.Any())
            {
                var roles = new List<Role>
                {
                    new() {Name = "User"},
                    new() {Name = "Administrator"},
                    new() {Name = "Employee"}
                };
                context.Role.AddRange(roles);
                context.SaveChanges();
            }
            if (!context.User.Any())
            {
                var users = new List<User.Domain.Models.Entities.User>
                {
                    new() { Username = "admin", Email = "admin@admin", PasswordHash = "password", Roles = [context.Role.First(r => r.Name == "Administrator")] } };
                context.User.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
