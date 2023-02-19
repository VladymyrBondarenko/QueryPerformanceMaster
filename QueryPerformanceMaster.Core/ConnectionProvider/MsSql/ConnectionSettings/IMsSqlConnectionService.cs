namespace QueryPerformanceMaster.Core.ConnectionProvider.MsSql.ConnectionSettings
{
    public interface IMsSqlConnectionService
    {
        string GetConnectionString(MsSqlConnectionSettings settings);
    }
}