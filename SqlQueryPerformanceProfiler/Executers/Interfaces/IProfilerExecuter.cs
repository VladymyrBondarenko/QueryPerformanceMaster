using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlQueryPerformanceProfiler.Executers.ExecResults;

namespace SqlQueryPerformanceProfiler.Executers.Interfaces
{
    public interface IProfilerExecuter
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(CancellationToken cancellationToken);
    }
}
