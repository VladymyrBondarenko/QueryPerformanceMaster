using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ConnectionProvider.MsSql
{
    public class MsSqlConnectionProvider : ConnectionProviderBase<SqlConnection>
    {
        private readonly string _connectionString;

        public MsSqlConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async override Task<SqlConnection> CreateConnection(CancellationToken cancellationToken = default)
        {
            var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync(cancellationToken);
            return sqlConnection;
        }
    }
}
