namespace SqlQueryPerformanceProfiler.Profilers.Interfaces
{
    public interface ILoadProfilersFactory
    {
        ILoadProfiler GetLoadProfiler(LoadProfilerParams sqlQueryLoadSettings);
    }
}