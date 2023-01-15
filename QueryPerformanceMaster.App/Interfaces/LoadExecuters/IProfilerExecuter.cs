using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters
{
    public interface IProfilerExecuter
    {
        Task<LoadExecutedResult> ExecuteLoadAsync(CancellationToken cancellationToken);
    }
}
