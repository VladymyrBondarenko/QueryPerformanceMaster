using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters
{
    public interface IProfilerExecuterService
    {
        Task<LoadExecutedResult> ExecuteParallelLoadAsync(string query, int threadNumber, int iterationNumber, SqlConnectionParams connectionParams, CancellationToken cancellationToken = default);
        Task<LoadExecutedResult> ExecuteSequentialLoadAsync(string query, int iterationNumber, SqlConnectionParams connectionParams, CancellationToken cancellationToken = default);
    }
}