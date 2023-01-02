using SqlQueryPerformanceProfiler.Executers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Profilers
{
    public class SqlConnectionParams
    {
        public string ConnectionString { get; set; }

        public SqlProvider SqlProvider { get; set; }
    }
}
