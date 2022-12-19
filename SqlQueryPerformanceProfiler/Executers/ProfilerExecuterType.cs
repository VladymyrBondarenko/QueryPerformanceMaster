using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Executers
{
    public enum ProfilerExecuterType
    {
        ParallerExecutor,
        SequentialExecutor,
        SequentialExecutorWithDelay
    }
}
