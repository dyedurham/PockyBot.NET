using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace PockyBot.NET.Persistence
{
    internal static class DatabaseExtensions
    {
        public static async Task SetUpOperationalUserPermissions(string adminConnectionString, string connectionString)
        {
            var dbConnectionValues = ParseConnectionString(connectionString);
            var username = dbConnectionValues["Username"].ToString();
            var password = dbConnectionValues["Password"].ToString();

            await using var adminConnection = new NpgsqlConnection(adminConnectionString);
            adminConnection.Open();
            if (!UserExists(adminConnection, username))
            {
                // Does not look like one can parameterise values in an create statement. String interpolation also causes strange behaviour.
                await adminConnection.ExecuteAsync($"CREATE USER {username} WITH PASSWORD '{password}'");
            }

            await GrantPermissionToUserAsync(adminConnection, username);
        }

        private static bool UserExists(NpgsqlConnection adminConnection, string username)
        {
            return adminConnection.Query<object>("SELECT 1 FROM pg_roles WHERE rolname = @username", new {username}).Any();
        }

        private static async Task GrantPermissionToUserAsync(NpgsqlConnection adminConnection, string userName)
        {
            await adminConnection.ExecuteAsync($"GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.pockyusers TO {userName}");
            await adminConnection.ExecuteAsync($"GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.pegs TO {userName}");
            await adminConnection.ExecuteAsync($"GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.generalconfig TO {userName}");
            await adminConnection.ExecuteAsync($"GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.roles TO {userName}");
            await adminConnection.ExecuteAsync($"GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.stringconfig TO {userName}");
            await adminConnection.ExecuteAsync($"GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.locations TO {userName}");
            await adminConnection.ExecuteAsync($"GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.user_locations TO {userName}");
            await adminConnection.ExecuteAsync($"GRANT SELECT, UPDATE ON ALL SEQUENCES IN SCHEMA public TO {userName}");
        }

        private static IDictionary<string, object> ParseConnectionString(string connectionString)
        {
            var values = connectionString.Split(";");
            return values.Select(v =>
            {
                var keyValue = v.Split("=", 2);
                return new KeyValuePair<string, object>(keyValue.First().Trim(), keyValue.Last().Trim());
            }).ToDictionary(k => k.Key,
                v => v.Value,
                StringComparer.CurrentCultureIgnoreCase);
        }
    }
}
