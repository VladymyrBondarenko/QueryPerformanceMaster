using QueryPerformanceMaster.Domain;

namespace QueryPerformanceMaster.App.Interfaces.SqlProviderServices
{
    public interface ISqlProviderServiceFactory
    {
        ISqlProviderService GetSqlProviderService(SqlConnectionParams connectionParams);
    }
}