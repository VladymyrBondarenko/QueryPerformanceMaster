using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.App.Interfaces.SqlProviderServices
{
    public interface ISqlProviderManagerFactory
    {
        ISqlProviderManager GetSqlProviderService(SqlConnectionParams connectionParams);
    }
}