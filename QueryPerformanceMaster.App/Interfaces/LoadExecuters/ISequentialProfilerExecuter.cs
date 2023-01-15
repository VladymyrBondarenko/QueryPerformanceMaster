using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters
{
    public interface ISequentialProfilerExecuter
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber,
            CancellationToken cancellationToken = default);
    }
}