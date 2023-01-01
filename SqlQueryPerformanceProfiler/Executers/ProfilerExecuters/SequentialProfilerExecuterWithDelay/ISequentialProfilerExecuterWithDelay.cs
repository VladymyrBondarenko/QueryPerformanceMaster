using SqlQueryPerformanceProfiler.Executers.ExecResults;

namespace SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.SequentialProfilerExecuterWithDelay
{
    public interface ISequentialProfilerExecuterWithDelay
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber, int delayMiliseconds,
            CancellationToken cancellationToken = default);
    }
}