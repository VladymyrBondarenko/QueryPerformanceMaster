using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.Core.SqlProviderServices.Factory
{
    public class SqlProviderServiceFactory : ISqlProviderServiceFactory
    {
        private readonly IMsSqlConnectionProviderFactory _msSqlconnectionProviderFactory;
        private readonly IPostgreSqlConnectionProviderFactory _postgreSqlconnectionProviderFactory;

        public SqlProviderServiceFactory(IMsSqlConnectionProviderFactory connectionProviderFactory,
            IPostgreSqlConnectionProviderFactory postgreSqlconnectionProviderFactory)
        {
            _msSqlconnectionProviderFactory = connectionProviderFactory;
            _postgreSqlconnectionProviderFactory = postgreSqlconnectionProviderFactory;
        }

        public ISqlProviderService GetSqlProviderService(SqlConnectionParams connectionParams)
        {
            switch (connectionParams.SqlProvider)
            {
                case SqlProvider.SqlServer:
                    return new MsSqlProviderService(_msSqlconnectionProviderFactory, connectionParams.ConnectionString);
                case SqlProvider.PostgreSql:
                    return new PostgreSqlProviderService(_postgreSqlconnectionProviderFactory, connectionParams.ConnectionString);
            }

            throw new NotImplementedException();
        }
    }
}
