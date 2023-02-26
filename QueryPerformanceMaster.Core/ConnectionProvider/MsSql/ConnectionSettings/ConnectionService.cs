using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ConnectionProvider.MsSql.ConnectionSettings
{
    public class ConnectionService : IConnectionService
    {
        private readonly IMsSqlConnectionService _msSqlConnectionService;

        public ConnectionService(IMsSqlConnectionService sqlConnectionService)
        {
            _msSqlConnectionService = sqlConnectionService;
        }

        public string SetDatabaseToConnectionString(SqlProvider sqlProvider, string connectionString, string database)
        {
            if (sqlProvider == SqlProvider.SqlServer)
            {
                var settings = _msSqlConnectionService.GetMsSqlConnectionSettings(connectionString);
                settings.Database = database;
                return _msSqlConnectionService.GetConnectionString(settings);
            }
            else if (sqlProvider == SqlProvider.PostgreSql)
            {
                // TODO: for PostgreSql
            }

            return string.Empty;
        }
    }
}
