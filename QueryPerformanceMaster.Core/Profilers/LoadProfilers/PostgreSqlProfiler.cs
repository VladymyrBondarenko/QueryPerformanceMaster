using Npgsql;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SqlQueryPerformanceProfiler.Profilers.LoadProfilers
{
    internal class PostgreSqlProfiler : ILoadProfiler
    {
        private readonly SqlConnectionParams _connectionParams;

        private static readonly Regex _queryLogicalReads = new Regex(
            @"Buffers: shared read=(\d{1,})?", 
            RegexOptions.Compiled);
        private static readonly Regex _queryTimes =
                new Regex(
                    @"Execution Time: (\d+\.\d+)? ms",
                    RegexOptions.Compiled);

        private const string _statisticsCommand = "EXPLAIN (ANALYZE, BUFFERS)";

        public PostgreSqlProfiler(SqlConnectionParams connectionParams)
        {
            _connectionParams = connectionParams;
        }

        public async Task<LoadProfilerResult> ExecuteQueryLoadAsync(string query, CancellationToken cancellationToken = default)
        {
            var sqlQueryLoadResult = new LoadProfilerResult();

            using var sqlConnection = new NpgsqlConnection(_connectionParams.ConnectionString);
            var sw = new Stopwatch();

            try
            {
                await sqlConnection.OpenAsync();

                using var cmd = sqlConnection.CreateCommand();
                cmd.CommandText = string.Concat(_statisticsCommand, " ", query);

                sw.Start();

                var reader = cmd.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    var textReader = await reader.GetTextReaderAsync(0, cancellationToken);
                    var queryPlanRow = await textReader.ReadLineAsync();

                    var matches = _queryTimes.Split(queryPlanRow);
                    if (matches.Length > 1)
                    {
                        sqlQueryLoadResult.ElapsedTime = Convert.ToDouble(matches[1], CultureInfo.InvariantCulture);
                        continue;
                    }

                    matches = _queryLogicalReads.Split(queryPlanRow);
                    if (matches.Length > 1)
                    {
                        sqlQueryLoadResult.LogicalReads = Convert.ToDouble(matches[1], CultureInfo.InvariantCulture);
                        continue;
                    }
                }

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
                    await sqlConnection.CloseAsync();
                }
            }

            sqlQueryLoadResult.ExecTime = sw.Elapsed;

            sw.Reset();

            return sqlQueryLoadResult;
        }
    }
}
