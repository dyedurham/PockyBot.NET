using Microsoft.EntityFrameworkCore;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasKey(table => new
            {
                table.UserId, table.UserRole
            });
        }

        public DbSet<PockyUser> PockyUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
