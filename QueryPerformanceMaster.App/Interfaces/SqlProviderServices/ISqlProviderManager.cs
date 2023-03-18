using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.App.Interfaces.SqlProviderServices
{
    public interface ISqlProviderService
    {
        Task<GetProviderDatabasesResult> GetSqlProviderDatabasesAsync();
    }
}