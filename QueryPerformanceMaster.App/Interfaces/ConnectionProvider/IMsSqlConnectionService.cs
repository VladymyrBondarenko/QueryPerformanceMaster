using QueryPerformanceMaster.Domain.ConnectionSettings;

namespace QueryPerformanceMaster.App.Interfaces.ConnectionProvider
{
    public interface IMsSqlConnectionService
    {
        string GetConnectionString(MsSqlConnectionSettings settings);
        MsSqlConnectionSettings GetMsSqlConnectionSettings(string connectionString);
    }
}