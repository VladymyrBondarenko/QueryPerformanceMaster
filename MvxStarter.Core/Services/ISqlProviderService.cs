using MvxStarter.Core.Models;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.Services
{
    public interface ISqlProviderManager
    {
        Task<DropBuffersAndCacheResult> DropBuffersAndCache(SqlProvider sqlProvider, string connectionString);
        Task<GetProviderDatabasesResult> GetSqlProviderDatabasesAsync(SqlProvider sqlProvider, string connectionString);
        List<SqlProviderModel> GetSqlProviders();
        Task OpenConnectionParamsView(SqlProvider sqlProvider, string connectionString = null);
    }
}