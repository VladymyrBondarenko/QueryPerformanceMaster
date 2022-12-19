using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;
using SqlQueryPerformanceProfiler.Profilers.Interfaces;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;

namespace SqlQueryPerformanceProfiler.Profilers
{
    internal class MsSqlLoadProfiler : ILoadProfiler
    {
        private readonly LoadProfilerParams _sqlQueryLoadParams;

        private static readonly Regex _queryLogicalReads = new Regex(
            @"(?:Table (\'\w{1,}\'|'#\w{1,}\'|'##\w{1,}\'). Scan count \d{1,}, logical reads )(\d{1,})", RegexOptions.Compiled);
        private static readonly Regex _queryTimes =
                new Regex(
                    @"(?:SQL Server Execution Times:|SQL Server parse and compile time:)(?:\s{1,}CPU time = )(\d{1,})(?: ms,\s{1,}elapsed time = )(\d{1,})",
                    RegexOptions.Compiled);

        private const string _statisticsCommand = "SET STATISTICS IO ON; SET STATISTICS TIME ON;";

        public MsSqlLoadProfiler(LoadProfilerParams sqlQueryLoadSettings)
        {
            _sqlQueryLoadParams = sqlQueryLoadSettings;
        }

        public async Task<LoadProfilerResult> ExecuteQueryLoadAsync(CancellationToken cancellationToken)
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

            // TODO: maybe change to pass sql connection from outside
            using var sqlConnection = new SqlConnection(_sqlQueryLoadParams.ConnectionString);
            var statCommand = new SqlCommand();
            var sw = new Stopwatch();

            try
            {
                await sqlConnection.OpenAsync();

                statCommand = sqlConnection.CreateCommand();
                statCommand.CommandText = _statisticsCommand;
                statCommand.Connection.InfoMessage += infoMessageHandler;
                await statCommand.ExecuteNonQueryAsync();

                var query = sqlConnection.CreateCommand();
                query.CommandText = _sqlQueryLoadParams.Query;

                sw.Start();

                await query.ExecuteNonQueryAsync();

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
