using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Profilers
{
    public class LoadProfilerExecuter
    {
        private readonly SqlQueryLoadParams _sqlQueryLoadParams;
        private readonly ILoadProfiler _loadProfiler;

        public LoadProfilerExecuter(SqlQueryLoadParams sqlQueryLoadSettings,
            ILoadProfiler loadProfiler)
        {
            _sqlQueryLoadParams = sqlQueryLoadSettings;
            _loadProfiler = loadProfiler;
        }

        public SqlQueryLoadResult ExecuteLoad()
        {
            if (string.IsNullOrWhiteSpace(_sqlQueryLoadParams.Query) ||
                string.IsNullOrWhiteSpace(_sqlQueryLoadParams.ConnectionString))
            {
                return null;
            }

            return _loadProfiler.ExecuteLoad();
        }
    }
}
