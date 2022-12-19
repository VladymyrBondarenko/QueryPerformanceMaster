using SqlQueryPerformanceProfiler.Executers.ExecResults;
using SqlQueryPerformanceProfiler.Executers.Interfaces;
using SqlQueryPerformanceProfiler.Profilers.Interfaces;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;
using SqlQueryPerformanceProfiler.Profilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SqlQueryPerformanceProfiler.Executers
{
    internal class ParallelProfilerExecuter : IProfilerExecuter
    {
        private readonly LoadProfilerParams _sqlQueryLoadParams;
        private readonly ILoadProfiler _loadProfiler;

        public ParallelProfilerExecuter(LoadProfilerParams sqlQueryLoadSettings,
            ILoadProfiler loadProfiler)
        {
            _sqlQueryLoadParams = sqlQueryLoadSettings;
            _loadProfiler = loadProfiler;
        }

        public async Task<LoadExecutedResult> ExecuteLoadAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_sqlQueryLoadParams.Query) ||
                string.IsNullOrWhiteSpace(_sqlQueryLoadParams.ConnectionString))
            {
                throw new ArgumentException("Invalid query load params!");
            }

            var tasks = new List<Task<List<LoadProfilerResult>>>();

            for (int i = 0; i < _sqlQueryLoadParams.ThreadsNumber; i++)
            {
                tasks.Add(ExecuteQueryLoad(cancellationToken));
            }

            Task.WaitAll(tasks.ToArray());

            var completedTasks = await Task.WhenAll(tasks);
            var loadProfilerResult = completedTasks.SelectMany(x => x).ToList();

            return ProfilerExecuterHelpers.FillLoadExecutedResult(_sqlQueryLoadParams, loadProfilerResult);
        }

        private async Task<List<LoadProfilerResult>> ExecuteQueryLoad(CancellationToken cancellationToken)
        {
            var loadProfilerResult = new List<LoadProfilerResult>();
            for (int i = 1; i <= _sqlQueryLoadParams.IterationsNumber; i++)
            {
                var loadResult = await _loadProfiler.ExecuteQueryLoadAsync(cancellationToken);
                loadProfilerResult.Add(loadResult);
            }
            return loadProfilerResult;
        }
    }
}
