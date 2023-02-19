using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.SqlProviderServices
{
    internal class MsSqlProviderManager : ISqlProviderManager
    {
        private readonly IMsSqlConnectionProviderFactory _connectionProviderFactory;
        private readonly string _connectionString;

        public MsSqlProviderManager(IMsSqlConnectionProviderFactory connectionProviderFactory, string connectionString)
        {
            _connectionProviderFactory = connectionProviderFactory;
            _connectionString = connectionString;
        }

        public async Task<List<SqlProviderDatabase>> GetSqlProviderDatabasesAsync()
        {
            const string query = "SELECT databases.name FROM sys.databases WHERE databases.state = 0 ORDER BY databases.name";

            var connectionProvider = _connectionProviderFactory.GetConnectionProvider(_connectionString);
            SqlConnection sqlConnection = null;
            var databases = new List<SqlProviderDatabase>();

            try
            {
                sqlConnection = await connectionProvider.CreateConnection();
                using var cmd = sqlConnection.CreateCommand();
                cmd.CommandText = query;

                var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    databases.Add(new SqlProviderDatabase { Name = (string)reader[0] });
                }
            }
            catch
            {
                // analyze exception codes
            }
            finally
            {
                if (sqlConnection != null)
                {
                    await sqlConnection.CloseAsync();
                }
            }

            return databases;
        }
    }
}
