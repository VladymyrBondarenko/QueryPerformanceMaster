using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Core.LoadProfilers.Profilers;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.SqlProviders;

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
                    return new MsSqlLoadProfiler(connectionParams, _msSqlConnectionProviderFactory);
                case SqlProvider.PostgreSql:
                    return new PostgreSqlLoadProfiler(connectionParams, _postgreSqlConnectionProviderFactory);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
