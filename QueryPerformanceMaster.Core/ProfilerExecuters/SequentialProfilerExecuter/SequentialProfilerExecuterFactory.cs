using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters.Factories;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;

namespace QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecuter
{
    public class SequentialProfilerExecuterFactory : ISequentialProfilerExecuterFactory
    {
        public ISequentialProfilerExecuter GetProfilerExecuter(ILoadProfiler loadProfiler)
        {
            var sequentialProfiler = new SequentialProfilerExecuter(loadProfiler);
            return sequentialProfiler;
        }
    }
}
