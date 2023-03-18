using MvxStarter.Core.Models;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.Services
{
    public interface ISqlProviderManager
    {
        Task<GetProviderDatabasesResult> GetSqlProviderDatabasesAsync(SqlProvider sqlProvider, string connectionString);
        List<SqlProviderModel> GetSqlProviders();
    }
}