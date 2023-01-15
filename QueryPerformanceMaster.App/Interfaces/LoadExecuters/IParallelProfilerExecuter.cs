using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters
{
    public interface IParallelProfilerExecuter
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int threadNumber, int iterationNumber,
            CancellationToken cancellationToken = default);
    }
}