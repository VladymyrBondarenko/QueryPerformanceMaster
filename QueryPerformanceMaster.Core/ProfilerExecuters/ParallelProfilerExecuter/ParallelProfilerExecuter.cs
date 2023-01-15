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
