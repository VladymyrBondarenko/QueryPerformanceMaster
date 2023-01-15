using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters
{
    public interface ISequentialProfilerExecutorWithTimeLimit
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber, int timeLimitMiliseconds, CancellationToken cancellationToken = default);
    }
}