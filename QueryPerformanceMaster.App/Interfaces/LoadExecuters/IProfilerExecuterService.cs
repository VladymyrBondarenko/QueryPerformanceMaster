using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters
{
    public interface IProfilerExecuterService
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(ProfilerExecuterType executerType, ExecuteLoadParmas executeLoadParmas, CancellationToken cancellationToken = default);
    }
}