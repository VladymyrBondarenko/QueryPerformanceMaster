using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.App.Interfaces.ConnectionProvider
{
    public interface IConnectionService
    {
        string SetDatabaseToConnectionString(SqlProvider sqlProvider, string connectionString, string database);
    }
}