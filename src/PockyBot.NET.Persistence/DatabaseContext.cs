using Microsoft.EntityFrameworkCore;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<PockyUser> PockyUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
