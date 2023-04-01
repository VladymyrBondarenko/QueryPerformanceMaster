using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters.Factories;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ProfilerExecuters.ParallelProfilerExecuter
{
    public class ParallelProfilerExecuterFactory : IParallelProfilerExecuterFactory
    {
        public IParallelProfilerExecuter GetProfilerExecuter(ILoadProfiler loadProfiler)
        {
            var sequentialProfiler = new ParallelProfilerExecuter(loadProfiler);
            return sequentialProfiler;
        }
    }
}
