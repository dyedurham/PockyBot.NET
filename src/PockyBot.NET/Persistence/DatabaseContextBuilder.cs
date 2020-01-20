using Microsoft.EntityFrameworkCore;

namespace PockyBot.NET.Persistence
{
    public static class DatabaseContextBuilder
    {
        public static DatabaseContext BuildDatabaseContext(string databaseConnectionString)
        {
            var dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(databaseConnectionString).Options;
            return new DatabaseContext(dbContextOptions);
        }
    }
}
