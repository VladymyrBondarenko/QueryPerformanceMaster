﻿using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters.Factories;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecuterWithDelay
{
    public class SequentialProfilerExecuterWithDelayFactory : ISequentialProfilerExecuterWithDelayFactory
    {
        public ISequentialProfilerExecuterWithDelay GetProfilerExecuter(ILoadProfiler loadProfiler)
        {
            var profiler = new SequentialProfilerExecuterWithDelay(loadProfiler);
            return profiler;
        }
    }
}
