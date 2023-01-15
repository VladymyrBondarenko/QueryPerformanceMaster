using QueryPerformanceMaster.Domain.LoadResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadProfilers
{
    public interface ILoadProfiler
    {
        Task<LoadProfilerResult> ExecuteQueryLoadAsync(string query, CancellationToken cancellationToken = default);
    }
}
