using Microsoft.EntityFrameworkCore;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasKey(table => new
            {
                table.UserId, table.UserRole
            });

            builder.Entity<StringConfig>().HasKey(table => new
            {
                table.Name, table.Value
            });

            builder.Entity<PockyUser>().HasOne(x => x.Location)
                .WithOne(x => x.User)
                .HasForeignKey<PockyUser>(x => x.UserId);
        }

        public DbSet<PockyUser> PockyUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Peg> Pegs { get; set; }
        public DbSet<GeneralConfig> GeneralConfig { get; set; }
        public DbSet<StringConfig> StringConfig { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
    }
}
