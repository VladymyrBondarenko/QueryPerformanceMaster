using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.Core.ConnectionProvider.PostgreSql;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.Core.ConnectionProvider
{
    public class ConnectionService : IConnectionService
    {
        private readonly IMsSqlConnectionService _msSqlConnectionService;
        private readonly IPostgreSqlConnectionService _postgreSqlConnectionService;

        public ConnectionService(IMsSqlConnectionService msSqlConnectionService, IPostgreSqlConnectionService postgreSqlConnectionService)
        {
            _msSqlConnectionService = msSqlConnectionService;
            _postgreSqlConnectionService = postgreSqlConnectionService;
        }

        public string SetDatabaseToConnectionString(SqlProvider sqlProvider, string connectionString, string database)
        {
            var resultConnectionString = string.Empty;

            if (sqlProvider == SqlProvider.SqlServer)
            {
                var settings = _msSqlConnectionService.GetMsSqlConnectionSettings(connectionString);
                settings.Database = database;

                resultConnectionString = _msSqlConnectionService.GetConnectionString(settings);
            }
            else if (sqlProvider == SqlProvider.PostgreSql)
            {
                var settings = _postgreSqlConnectionService.GetPostgreSqlConnectionSettings(connectionString);
                settings.Database = database;

                resultConnectionString = _postgreSqlConnectionService.GetConnectionString(settings);
            }

            return resultConnectionString;
        }
    }
}
