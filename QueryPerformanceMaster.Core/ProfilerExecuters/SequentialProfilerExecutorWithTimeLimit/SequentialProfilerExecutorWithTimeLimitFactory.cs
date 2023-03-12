using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters.Factories;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecutorWithTimeLimit
{
    public class SequentialProfilerExecutorWithTimeLimitFactory : ISequentialProfilerExecutorWithTimeLimitFactory
    {
        public ISequentialProfilerExecutorWithTimeLimit GetSequentialProfilerExecuter(ILoadProfiler loadProfiler)
        {
            var profiler = new SequentialProfilerExecutorWithTimeLimit(loadProfiler);
            return profiler;
        }
    }
}
