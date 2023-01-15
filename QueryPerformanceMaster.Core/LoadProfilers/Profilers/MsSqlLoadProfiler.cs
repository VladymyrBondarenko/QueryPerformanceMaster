using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.LoadResults;

namespace QueryPerformanceMaster.Core.LoadProfilers.Profilers
{
    internal class MsSqlLoadProfiler : ILoadProfiler
    {
        private readonly SqlConnectionParams _connectionParams;
        private readonly IMsSqlConnectionProviderFactory _connectionProviderFactory;
        private static readonly Regex _queryLogicalReads = new Regex(
            @"(?:Table (\'\w{1,}\'|'#\w{1,}\'|'##\w{1,}\'). Scan count \d{1,}, logical reads )(\d{1,})", RegexOptions.Compiled);
        private static readonly Regex _queryTimes =
                new Regex(
                    @"(?:SQL Server Execution Times:|SQL Server parse and compile time:)(?:\s{1,}CPU time = )(\d{1,})(?: ms,\s{1,}elapsed time = )(\d{1,})",
                    RegexOptions.Compiled);

        private const string _statisticsCommand = "SET STATISTICS IO ON; SET STATISTICS TIME ON;";

        public MsSqlLoadProfiler(SqlConnectionParams connectionParams,
            IMsSqlConnectionProviderFactory connectionProviderFactory)
        {
            _connectionParams = connectionParams;
            _connectionProviderFactory = connectionProviderFactory;
        }

        public async Task<LoadProfilerResult> ExecuteQueryLoadAsync(string query,
            CancellationToken cancellationToken = default)
        {
            var sqlQueryLoadResult = new LoadProfilerResult();

            SqlInfoMessageEventHandler infoMessageHandler = (sender, e) =>
            {
                foreach (SqlError erorr in e.Errors)
                {
                    var matches = _queryLogicalReads.Split(erorr.Message);

                    if (matches.Length > 1)
                    {
                        sqlQueryLoadResult.LogicalReads = Convert.ToInt32(matches[2], CultureInfo.InvariantCulture);
                        continue;
                    }

                    matches = _queryTimes.Split(erorr.Message);

                    if (matches.Length > 1)
                    {
                        sqlQueryLoadResult.CpuTime = Convert.ToInt32(matches[1], CultureInfo.InvariantCulture);
                        sqlQueryLoadResult.ElapsedTime = Convert.ToInt32(matches[2], CultureInfo.InvariantCulture);
                    }
                }
            };

            var sw = new Stopwatch();
            var connectionProvider = _connectionProviderFactory.GetConnectionProvider(_connectionParams.ConnectionString);
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = await connectionProvider.CreateConnection();

                using var statCommand = sqlConnection.CreateCommand();
                statCommand.CommandText = _statisticsCommand;
                statCommand.Connection.InfoMessage += infoMessageHandler;
                await statCommand.ExecuteNonQueryAsync();

                using var cmd = sqlConnection.CreateCommand();
                cmd.CommandText = query;

                sw.Start();

                await cmd.ExecuteNonQueryAsync();

                sw.Stop();
            }
            catch (Exception ex)
            {
                sqlQueryLoadResult.SqlQueryLoadError = ex.Message;
                sw.Stop();
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.InfoMessage -= infoMessageHandler;
                    await sqlConnection.CloseAsync();
                }
            }

            sqlQueryLoadResult.ExecTime = sw.Elapsed;

            sw.Reset();

            return sqlQueryLoadResult;
        }
    }
}
