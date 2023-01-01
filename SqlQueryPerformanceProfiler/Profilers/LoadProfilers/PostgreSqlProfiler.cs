using SqlQueryPerformanceProfiler.Profilers.LoadResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Profilers.LoadProfilers
{
    internal class PostgreSqlProfiler : ILoadProfiler
    {
        public Task<LoadProfilerResult> ExecuteQueryLoadAsync(string query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
