using SqlQueryPerformanceProfiler.Profilers.Interfaces;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Profilers
{
    internal class PostgreSqlProfiler : ILoadProfiler
    {
        public Task<LoadProfilerResult> ExecuteQueryLoadAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
