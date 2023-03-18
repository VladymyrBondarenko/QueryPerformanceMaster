using Npgsql;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.Core.SqlProviderServices
{
    internal class PostgreSqlProviderService : ISqlProviderService
    {
        private readonly IPostgreSqlConnectionProviderFactory _connectionProviderFactory;
        private readonly string _connectionString;

        public PostgreSqlProviderService(IPostgreSqlConnectionProviderFactory connectionProviderFactory, string connectionString)
        {
            _connectionProviderFactory = connectionProviderFactory;
            _connectionString = connectionString;
        }

        public async Task<GetProviderDatabasesResult> GetSqlProviderDatabasesAsync()
        {
            const string query = "SELECT datname FROM pg_database WHERE datistemplate = false ORDER BY datname";

            var connectionProvider = _connectionProviderFactory.GetConnectionProvider(_connectionString);
            NpgsqlConnection sqlConnection = null;
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
            catch (Exception ex)
            {
                return new GetProviderDatabasesResult(false)
                {
                    ErrorMessage = ex.Message
                };
            }
            finally
            {
                if (sqlConnection != null)
                {
                    await sqlConnection.CloseAsync();
                }
            }

            return new GetProviderDatabasesResult { SqlProviderDatabases = databases };
        }
    }
}
