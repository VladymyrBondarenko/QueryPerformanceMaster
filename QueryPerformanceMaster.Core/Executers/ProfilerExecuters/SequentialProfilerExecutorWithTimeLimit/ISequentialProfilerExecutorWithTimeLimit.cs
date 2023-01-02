using SqlQueryPerformanceProfiler.Executers.ExecResults;

namespace SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.SequentialProfilerExecutorWithTimeLimit
{
    public interface ISequentialProfilerExecutorWithTimeLimit
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber, int timeLimitMiliseconds, CancellationToken cancellationToken = default);
    }
}