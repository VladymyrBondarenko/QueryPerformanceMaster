using SqlQueryPerformanceProfiler.Executers.ExecResults;

namespace SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.ParallelProfilerExecuter
{
    public interface IParallelProfilerExecuter
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int threadNumber, int iterationNumber, 
            CancellationToken cancellationToken = default);
    }
}