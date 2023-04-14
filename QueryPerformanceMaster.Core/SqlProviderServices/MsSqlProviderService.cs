using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Domain.SqlProviders;
using System.Data.SqlClient;

namespace QueryPerformanceMaster.Core.SqlProviderServices
{
    internal class MsSqlProviderService : ISqlProviderService
    {
        private readonly IMsSqlConnectionProviderFactory _connectionProviderFactory;
        private readonly string _connectionString;

        public MsSqlProviderService(IMsSqlConnectionProviderFactory connectionProviderFactory, string connectionString)
        {
            _connectionProviderFactory = connectionProviderFactory;
            _connectionString = connectionString;
        }

        public async Task<DropBuffersAndCacheResult> DropBuffersAndCache()
        {
            const string query = 
                @"CHECKPOINT
                DBCC DROPCLEANBUFFERS
                DBCC FREEPROCCACHE
                ";

            var connectionProvider = _connectionProviderFactory.GetConnectionProvider(_connectionString);
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = await connectionProvider.CreateConnection();
                using var cmd = sqlConnection.CreateCommand();
                cmd.CommandText = query;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                return new DropBuffersAndCacheResult(false)
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

            return new DropBuffersAndCacheResult();
        }

        public async Task<GetProviderDatabasesResult> GetSqlProviderDatabasesAsync()
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
            catch(Exception ex)
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
