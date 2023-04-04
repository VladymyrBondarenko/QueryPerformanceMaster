using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Domain.ExecResults;
using QueryPerformanceMaster.Domain.LoadResults;

namespace QueryPerformanceMaster.Core.ProfilerExecuters.ParallelProfilerExecuter
{
    public class ParallelProfilerExecuter : IParallelProfilerExecuter
    {
        private readonly ILoadProfiler _loadProfiler;

        public ParallelProfilerExecuter(ILoadProfiler loadProfiler)
        {
            _loadProfiler = loadProfiler;
        }

        public async Task<LoadExecutedResult> ExecuteLoadAsync(string query, int threadNumber, int iterationNumber,
            IProgress<int> queryLoadProgress = null, CancellationToken cancellationToken = default)
        {
            var tasks = new List<Task<List<LoadProfilerResult>>>();

            for (int i = 1; i <= threadNumber; i++)
            {
                tasks.Add(ExecuteQueryLoad(query, iterationNumber,
                    threadNumber * iterationNumber, cancellationToken, queryLoadProgress));
            }

            var completedTasks = await Task.WhenAll(tasks);
            var loadProfilerResult = completedTasks.SelectMany(x => x).ToList();

            return ProfilerExecuterHelpers.FillLoadExecutedResult(loadProfilerResult.Count, loadProfilerResult);
        }

        private int _currentLoadProgress;
        private async Task<List<LoadProfilerResult>> ExecuteQueryLoad(string query, int iterationNumber,
            int finalIterationNumber, CancellationToken cancellationToken, IProgress<int> queryLoadProgress = null)
        {
            var loadProfilerResult = new List<LoadProfilerResult>();
            for (int i = 1; i <= iterationNumber; i++)
            {
                queryLoadProgress?.Report((++_currentLoadProgress * 100) / finalIterationNumber);

                var loadResult = await _loadProfiler.ExecuteQueryLoadAsync(query, cancellationToken);
                loadProfilerResult.Add(loadResult);

                if (cancellationToken.IsCancellationRequested)
                {
                    return loadProfilerResult;
                }
            }
            return loadProfilerResult;
        }
    }
}
