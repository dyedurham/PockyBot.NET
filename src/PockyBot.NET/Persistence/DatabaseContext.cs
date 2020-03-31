using Microsoft.EntityFrameworkCore;
using Npgsql;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        static DatabaseContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Role>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserRole>().HasKey(table => new
            {
                table.UserId, UserRole = table.Role
            });

            builder.Entity<StringConfig>().HasKey(table => new
            {
                table.Name, table.Value
            });

            builder.Entity<PockyUser>().HasOne(x => x.Location)
                .WithOne(x => x.User)
                .HasForeignKey<UserLocation>(x => x.UserId);
        }

        public DbSet<PockyUser> PockyUsers { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<Peg> Pegs { get; set; }
        public DbSet<GeneralConfig> GeneralConfig { get; set; }
        public DbSet<StringConfig> StringConfig { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
    }
}
