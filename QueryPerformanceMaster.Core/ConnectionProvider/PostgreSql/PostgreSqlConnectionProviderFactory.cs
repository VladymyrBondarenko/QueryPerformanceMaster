using Npgsql;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ConnectionProvider.PostgreSql
{
    public class PostgreSqlConnectionProviderFactory : IPostgreSqlConnectionProviderFactory
    {
        public IConnectionProvider<NpgsqlConnection> GetConnectionProvider(string connectionString)
        {
            var connectionProvider = new PostgreSqlConnectionProvider(connectionString);
            return connectionProvider;
        }
    }
}
