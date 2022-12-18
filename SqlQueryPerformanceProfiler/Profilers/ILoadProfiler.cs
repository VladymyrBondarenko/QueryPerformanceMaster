using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Profilers
{
    public interface ILoadProfiler
    {
        SqlQueryLoadResult ExecuteLoad();
    }
}
