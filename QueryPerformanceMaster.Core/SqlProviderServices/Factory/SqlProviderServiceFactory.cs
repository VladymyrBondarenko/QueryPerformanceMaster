using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.Core.SqlProviderServices.Factory
{
    public class SqlProviderManagerFactory : ISqlProviderManagerFactory
    {
        private readonly IMsSqlConnectionProviderFactory _connectionProviderFactory;

        public SqlProviderManagerFactory(IMsSqlConnectionProviderFactory connectionProviderFactory)
        {
            _connectionProviderFactory = connectionProviderFactory;
        }

        public ISqlProviderManager GetSqlProviderService(SqlConnectionParams connectionParams)
        {
            switch (connectionParams.SqlProvider)
            {
                case SqlProvider.SqlServer:
                    return new MsSqlProviderManager(_connectionProviderFactory, connectionParams.ConnectionString);
            }

            throw new NotImplementedException();
        }
    }
}
