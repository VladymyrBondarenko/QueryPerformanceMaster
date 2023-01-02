using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;

namespace SqlQueryPerformanceProfiler.Profilers.LoadProfilers
{
    public interface ILoadProfiler
    {
        Task<LoadProfilerResult> ExecuteQueryLoadAsync(string query, CancellationToken cancellationToken = default);
    }
}
