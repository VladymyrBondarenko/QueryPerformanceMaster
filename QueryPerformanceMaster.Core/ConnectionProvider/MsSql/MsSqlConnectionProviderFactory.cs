using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ConnectionProvider.MsSql
{
    public class MsSqlConnectionProviderFactory : IMsSqlConnectionProviderFactory
    {
        public IConnectionProvider<SqlConnection> GetConnectionProvider(string connectionString)
        {
            var connectionProvider = new MsSqlConnectionProvider(connectionString);
            return connectionProvider;
        }
    }
}
