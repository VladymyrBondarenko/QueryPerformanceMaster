using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.App.Interfaces.SqlProviderServices
{
    public interface ISqlProviderService
    {
        Task<DropBuffersAndCacheResult> DropBuffersAndCache();
        Task<GetProviderDatabasesResult> GetSqlProviderDatabasesAsync();
    }
}