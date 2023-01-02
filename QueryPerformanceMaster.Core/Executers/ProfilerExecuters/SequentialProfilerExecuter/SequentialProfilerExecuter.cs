using MathNet.Numerics.Statistics;
using SqlQueryPerformanceProfiler.Executers.ExecResults;
using SqlQueryPerformanceProfiler.Profilers;
using SqlQueryPerformanceProfiler.Profilers.LoadProfilers;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.SequentialProfilerExecuter
{
    public class SequentialProfilerExecuter : ISequentialProfilerExecuter
    {
        private readonly ILoadProfiler _loadProfiler;

        public SequentialProfilerExecuter(ILoadProfiler loadProfiler)
        {
            _loadProfiler = loadProfiler;
        }

        public async Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber, 
            CancellationToken cancellationToken = default)
        {
            var loadProfilerResult = new List<LoadProfilerResult>();

            for (int i = 1; i <= iterationNumber; i++)
            {
                var loadResult = await _loadProfiler.ExecuteQueryLoadAsync(query, cancellationToken);
                loadProfilerResult.Add(loadResult);
            }

            return ProfilerExecuterHelpers.FillLoadExecutedResult(iterationNumber, loadProfilerResult);
        }
    }
}
