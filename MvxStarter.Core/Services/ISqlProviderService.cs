using MvxStarter.Core.Models;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.Services
{
    public interface ISqlProviderService
    {
        Task<List<SqlProviderDatabase>> GetSqlProviderDatabasesAsync(SqlProvider sqlProvider, string connectionString);
        List<SqlProviderModel> GetSqlProviders();
    }
}