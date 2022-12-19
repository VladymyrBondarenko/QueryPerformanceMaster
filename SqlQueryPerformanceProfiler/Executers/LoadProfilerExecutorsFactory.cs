using SqlQueryPerformanceProfiler.Executers.Interfaces;
using SqlQueryPerformanceProfiler.Profilers;
using SqlQueryPerformanceProfiler.Profilers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Executers
{
    public class LoadProfilerExecutorsFactory : ILoadProfilerExecutorsFactory
    {
        private readonly ILoadProfilersFactory _loadProfilersFactory;

        public LoadProfilerExecutorsFactory(ILoadProfilersFactory loadProfilersFactory)
        {
            _loadProfilersFactory = loadProfilersFactory;
        }

        public IProfilerExecuter GetProfilerExecuter(LoadProfilerParams sqlQueryLoadSettings)
        {
            var loadProfiler = _loadProfilersFactory.GetLoadProfiler(sqlQueryLoadSettings);

            switch (sqlQueryLoadSettings.ExecuterType)
            {
                case ProfilerExecuterType.ParallerExecutor:
                    return (IProfilerExecuter)Activator.CreateInstance(typeof(ParallelProfilerExecuter), sqlQueryLoadSettings, loadProfiler);
                case ProfilerExecuterType.SequentialExecutor:
                    return (IProfilerExecuter)Activator.CreateInstance(typeof(SequentialProfilerExecuterWithDelay), sqlQueryLoadSettings, loadProfiler);
                case ProfilerExecuterType.SequentialExecutorWithDelay:
                    return (IProfilerExecuter)Activator.CreateInstance(typeof(SequentialProfilerExecuter), sqlQueryLoadSettings, loadProfiler);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
