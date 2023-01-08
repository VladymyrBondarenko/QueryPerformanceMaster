using SqlQueryPerformanceProfiler.Executers.ExecResults;
using SqlQueryPerformanceProfiler.Profilers.LoadProfilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Profilers
{
    public class LoadProfilersFactory : ILoadProfilersFactory
    {
        public ILoadProfiler GetLoadProfiler(SqlConnectionParams connectionParams)
        {
            switch (connectionParams.SqlProvider)
            {
                case SqlProvider.SqlServer:
                    return (ILoadProfiler)Activator.CreateInstance(typeof(MsSqlLoadProfiler), connectionParams);
                case SqlProvider.PostgreSql:
                    return (ILoadProfiler)Activator.CreateInstance(typeof(PostgreSqlProfiler), connectionParams);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
