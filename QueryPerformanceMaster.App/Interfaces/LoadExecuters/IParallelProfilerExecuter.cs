using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters
{
    public interface IParallelProfilerExecuter
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int threadNumber, int iterationNumber,
            IProgress<int>? queryLoadProgress = null, CancellationToken cancellationToken = default);
    }
}