using SqlQueryPerformanceProfiler.Executers.ExecResults;
using SqlQueryPerformanceProfiler.Profilers;
using SqlQueryPerformanceProfiler.Profilers.LoadProfilers;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.SequentialProfilerExecuterWithDelay
{
    public class SequentialProfilerExecuterWithDelay : ISequentialProfilerExecuterWithDelay
    {
        private readonly ILoadProfiler _loadProfiler;

        public SequentialProfilerExecuterWithDelay(ILoadProfiler loadProfiler)
        {
            _loadProfiler = loadProfiler;
        }

        public async Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber, int delayMiliseconds,
            CancellationToken cancellationToken = default)
        {
            var loadProfilerResult = new List<LoadProfilerResult>();

            for (int i = 1; i <= iterationNumber; i++)
            {
                await Task.Delay(delayMiliseconds);
                var loadResult = await _loadProfiler.ExecuteQueryLoadAsync(query, cancellationToken);
                loadProfilerResult.Add(loadResult);
            }

            return ProfilerExecuterHelpers.FillLoadExecutedResult(iterationNumber, loadProfilerResult);
        }
    }
}
