using SqlQueryPerformanceProfiler.Executers.ExecResults;
using SqlQueryPerformanceProfiler.Profilers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Profilers
{
    public class LoadProfilersFactory : ILoadProfilersFactory
    {
        public ILoadProfiler GetLoadProfiler(LoadProfilerParams sqlQueryLoadSettings)
        {
            switch (sqlQueryLoadSettings.SqlProvider)
            {
                case SqlProvider.SqlServer:
                    return (ILoadProfiler)Activator.CreateInstance(typeof(MsSqlLoadProfiler), sqlQueryLoadSettings);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
