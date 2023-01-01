using SqlQueryPerformanceProfiler.Executers.ExecResults;

namespace SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.SequentialProfilerExecuter
{
    public interface ISequentialProfilerExecuter
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber,
            CancellationToken cancellationToken = default);
    }
}