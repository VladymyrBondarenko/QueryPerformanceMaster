using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Domain.ExecResults;
using QueryPerformanceMaster.Domain.LoadResults;

namespace QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecuterWithDelay
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
