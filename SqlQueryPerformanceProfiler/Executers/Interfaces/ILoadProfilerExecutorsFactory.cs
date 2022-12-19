using SqlQueryPerformanceProfiler.Profilers;

namespace SqlQueryPerformanceProfiler.Executers.Interfaces
{
    public interface ILoadProfilerExecutorsFactory
    {
        IProfilerExecuter GetProfilerExecuter(LoadProfilerParams sqlQueryLoadSettings);
    }
}