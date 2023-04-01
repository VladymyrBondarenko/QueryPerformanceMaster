using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;

namespace QueryPerformanceMaster.App.Interfaces.LoadExecuters.Factories
{
    public interface ISequentialProfilerExecuterFactory
    {
        ISequentialProfilerExecuter GetProfilerExecuter(ILoadProfiler loadProfiler);
    }
}