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

namespace SqlQueryPerformanceProfiler.Profilers
{
    public class MssqlLoadProfiler : ILoadProfiler
    {
        private readonly SqlQueryLoadParams _sqlQueryLoadParams;

        private static readonly Regex _queryLogicalReads = new Regex(
            @"(?:Table (\'\w{1,}\'|'#\w{1,}\'|'##\w{1,}\'). Scan count \d{1,}, logical reads )(\d{1,})", RegexOptions.Compiled);
        private static readonly Regex _queryTimes =
                new Regex(
                    @"(?:SQL Server Execution Times:|SQL Server parse and compile time:)(?:\s{1,}CPU time = )(\d{1,})(?: ms,\s{1,}elapsed time = )(\d{1,})",
                    RegexOptions.Compiled);

        private const string _statisticsCommand = "SET STATISTICS IO ON; SET STATISTICS TIME ON;";

        public MssqlLoadProfiler(SqlQueryLoadParams sqlQueryLoadSettings)
        {
            _sqlQueryLoadParams = sqlQueryLoadSettings;
        }

        public SqlQueryLoadResult ExecuteLoad()
        {
            if (string.IsNullOrWhiteSpace(_sqlQueryLoadParams.Query) ||
                string.IsNullOrWhiteSpace(_sqlQueryLoadParams.ConnectionString))
            {
                return null;
            }

            var sqlQueryLoadResult = new SqlQueryLoadResult();

            SqlInfoMessageEventHandler infoMessageHandler = (sender, e) =>
            {
                foreach (SqlError erorr in e.Errors)
                {
                    var matches = _queryLogicalReads.Split(erorr.Message);

                    if (matches.Length > 1)
                    {
                        sqlQueryLoadResult.LogicalReads.Add(Convert.ToInt32(matches[2], CultureInfo.InvariantCulture));
                        continue;
                    }

                    matches = _queryTimes.Split(erorr.Message);

                    if (matches.Length > 1)
                    {
                        sqlQueryLoadResult.CpuTimes.Add(Convert.ToInt32(matches[1], CultureInfo.InvariantCulture));
                        sqlQueryLoadResult.ElapsedTimes.Add(Convert.ToInt32(matches[2], CultureInfo.InvariantCulture));
                    }
                }
            };

            using var sqlConnection = new SqlConnection(_sqlQueryLoadParams.ConnectionString);
            var statCommand = new SqlCommand();
            var sw = new Stopwatch();

            for (int i = 0; i < _sqlQueryLoadParams.IterationsNumber; i++)
            {
                try
                {
                    sqlConnection.Open();

                    statCommand = sqlConnection.CreateCommand();
                    statCommand.CommandText = _statisticsCommand;
                    statCommand.Connection.InfoMessage += infoMessageHandler;
                    statCommand.ExecuteNonQuery();

                    var query = sqlConnection.CreateCommand();
                    query.CommandText = _sqlQueryLoadParams.Query;

                    sw.Start();

                    query.ExecuteNonQuery();

                    sw.Stop();
                }
                catch (Exception ex)
                {
                    sqlQueryLoadResult.SqlQueryLoadErrors.Add(new SqlQueryLoadError { ErrorMessage = ex.Message });
                    sw.Stop();
                }
                finally
                {
                    if (sqlConnection != null)
                    {
                        sqlConnection.InfoMessage -= infoMessageHandler;
                        sqlConnection.Close();
                    }
                }

                sqlQueryLoadResult.ExecTime += sw.Elapsed;

                sw.Reset();
            }

            return FillQueryLoadResult(sqlQueryLoadResult);
        }

        private SqlQueryLoadResult FillQueryLoadResult(SqlQueryLoadResult queryLoadResult)
        {
            // calc total
            queryLoadResult.CpuTimeTotal = queryLoadResult.CpuTimes.Sum() / 1000;
            queryLoadResult.LogicalReadsTotal = queryLoadResult.LogicalReads.Sum();

            // calc avg
            queryLoadResult.CpuTimeAvg = queryLoadResult.CpuTimeTotal / _sqlQueryLoadParams.IterationsNumber;
            queryLoadResult.LogicalReadsAvg = queryLoadResult.LogicalReadsTotal / _sqlQueryLoadParams.IterationsNumber;

            // calc time
            queryLoadResult.ElapsedTimeTotal = queryLoadResult.ElapsedTimes.Sum() / 1000;

            // calc mod
            queryLoadResult.CpuTimeMod = queryLoadResult.CpuTimes.Median();
            queryLoadResult.LogicalReadsMod = queryLoadResult.LogicalReads.Median();
            queryLoadResult.ElapsedTimeMod = queryLoadResult.ElapsedTimes.Median();

            // calc errors
            var groupedErrors = queryLoadResult.SqlQueryLoadErrors.GroupBy(x => x.ErrorMessage);

            queryLoadResult.SqlQueryLoadErrors = new List<SqlQueryLoadError>();
            foreach (var error in groupedErrors)
            {
                queryLoadResult.SqlQueryLoadErrors.Add(new SqlQueryLoadError
                {
                    ErrorMessage = error.Key,
                    Count = error.Count()
                });
            }

            queryLoadResult.IterationCompleted = _sqlQueryLoadParams.IterationsNumber;

            return queryLoadResult;
        }
    }
}
