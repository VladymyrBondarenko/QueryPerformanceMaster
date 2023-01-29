using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Core.LoadProfilers.Profilers;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.SqlProviders;
using System.Data;

namespace QueryPerformanceMaster.Core.LoadProfilers
{
    public class LoadProfilersFactory : ILoadProfilersFactory
    {
        private readonly IMsSqlConnectionProviderFactory _msSqlConnectionProviderFactory;
        private readonly IPostgreSqlConnectionProviderFactory _postgreSqlConnectionProviderFactory;

        public LoadProfilersFactory(IMsSqlConnectionProviderFactory msSqlConnectionProviderFactory,
            IPostgreSqlConnectionProviderFactory postgreSqlConnectionProviderFactory)
        {
            _msSqlConnectionProviderFactory = msSqlConnectionProviderFactory;
            _postgreSqlConnectionProviderFactory = postgreSqlConnectionProviderFactory;
        }

        public ILoadProfiler GetLoadProfiler(SqlConnectionParams connectionParams)
        {
            switch (connectionParams.SqlProvider)
            {
                case SqlProvider.SqlServer:
                    return (ILoadProfiler)Activator.CreateInstance(typeof(MsSqlLoadProfiler), connectionParams, _msSqlConnectionProviderFactory);
                case SqlProvider.PostgreSql:
                    return (ILoadProfiler)Activator.CreateInstance(typeof(PostgreSqlLoadProfiler), connectionParams, _postgreSqlConnectionProviderFactory);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
