using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ConnectionProvider.PostgreSql
{
    internal class PostgreSqlConnectionProvider : ConnectionProviderBase<NpgsqlConnection>
    {
        private readonly string _connectionString;

        public PostgreSqlConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async override Task<NpgsqlConnection> CreateConnection()
        {
            var sqlConnection = new NpgsqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }
    }
}
