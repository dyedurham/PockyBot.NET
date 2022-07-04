using System.Reflection;
using DbUp;
using DbUp.Engine;

namespace PockyBot.NET.Persistence
{
    public class DatabaseUpdater
    {
        private readonly string _databaseAdminConnectionString;
        private readonly string _nonAdminDbConnectionString;

        public DatabaseUpdater(string databaseAdminConnectionString, string nonAdminDbConnectionString)
        {
            _databaseAdminConnectionString = databaseAdminConnectionString;
            _nonAdminDbConnectionString = nonAdminDbConnectionString;
        }

        public void RunScripts()
        {
            RunScripts("PockyBot.NET.Persistence.Migrations");
        }

        public void RunScripts(bool mark)
        {
            RunScripts("PockyBot.NET.Persistence.Migrations", mark);
        }

        private void RunScripts(string scriptNamespace, bool mark = false)
        {
            EnsureDatabase.For.PostgresqlDatabase(_databaseAdminConnectionString);

            var databaseUpdater = DeployChanges.To
                .PostgresqlDatabase(_databaseAdminConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script => script.StartsWith(scriptNamespace))
                .Build();

            DatabaseUpgradeResult result;
            if (!mark)
            {
                result = databaseUpdater.PerformUpgrade();
            }
            else
            {
                result = databaseUpdater.MarkAsExecuted();
            }

            if (!result.Successful)
            {
                throw result.Error;
            }
            DatabaseExtensions.SetUpOperationalUserPermissions(_databaseAdminConnectionString, _nonAdminDbConnectionString).Wait();
        }
    }
}
