using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters
{
    public interface ISequentialProfilerExecuterWithDelay
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber, int delayMiliseconds,
            IProgress<int>? queryLoadProgress = null, CancellationToken cancellationToken = default);
    }
}