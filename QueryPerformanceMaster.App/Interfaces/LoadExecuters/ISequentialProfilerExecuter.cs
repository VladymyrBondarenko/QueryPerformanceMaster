using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters
{
    public interface ISequentialProfilerExecuter
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber,
            IProgress<int>? queryLoadProgress = null, CancellationToken cancellationToken = default);
    }
}