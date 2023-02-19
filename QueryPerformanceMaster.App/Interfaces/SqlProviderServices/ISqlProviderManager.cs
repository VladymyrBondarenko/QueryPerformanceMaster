using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.App.Interfaces.SqlProviderServices
{
    public interface ISqlProviderManager
    {
        Task<List<SqlProviderDatabase>> GetSqlProviderDatabasesAsync();
    }
}