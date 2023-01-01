using SqlQueryPerformanceProfiler.Profilers.LoadProfilers;

namespace SqlQueryPerformanceProfiler.Profilers
{
    public interface ILoadProfilersFactory
    {
        ILoadProfiler GetLoadProfiler(SqlConnectionParams connectionParams);
    }
}