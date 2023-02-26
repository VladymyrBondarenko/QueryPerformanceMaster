using MvxStarter.Core.Models;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.Services
{
    public interface ISqlProviderService
    {
        Task<GetProviderDatabasesResult> GetSqlProviderDatabasesAsync(SqlProvider sqlProvider, string connectionString);
        List<SqlProviderModel> GetSqlProviders();
    }
}