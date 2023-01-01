using SqlQueryPerformanceProfiler.Executers.ExecResults;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;
using SqlQueryPerformanceProfiler.Profilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SqlQueryPerformanceProfiler.Profilers.LoadProfilers;

namespace SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.ParallelProfilerExecuter
{
    public class ParallelProfilerExecuter : IParallelProfilerExecuter
    {
        private readonly ILoadProfiler _loadProfiler;

        public ParallelProfilerExecuter(ILoadProfiler loadProfiler)
        {
            _loadProfiler = loadProfiler;
        }

        public async Task<LoadExecutedResult> ExecuteLoadAsync(string query, int threadNumber, int iterationNumber,
            CancellationToken cancellationToken = default)
        {
            var tasks = new List<Task<List<LoadProfilerResult>>>();

            for (int i = 0; i < threadNumber; i++)
            {
                tasks.Add(ExecuteQueryLoad(query, 
                    iterationNumber, cancellationToken));
            }

            Task.WaitAll(tasks.ToArray());

            var completedTasks = await Task.WhenAll(tasks);
            var loadProfilerResult = completedTasks.SelectMany(x => x).ToList();

            return ProfilerExecuterHelpers.FillLoadExecutedResult(iterationNumber, loadProfilerResult);
        }

        private async Task<List<LoadProfilerResult>> ExecuteQueryLoad(string query, int iterationNumber, 
            CancellationToken cancellationToken)
        {
            var loadProfilerResult = new List<LoadProfilerResult>();
            for (int i = 1; i <= iterationNumber; i++)
            {
                var loadResult = await _loadProfiler.ExecuteQueryLoadAsync(query, cancellationToken);
                loadProfilerResult.Add(loadResult);
            }
            return loadProfilerResult;
        }
    }
}
