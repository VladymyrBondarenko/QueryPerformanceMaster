using Npgsql;

namespace QueryPerformanceMaster.App.Interfaces.ConnectionProvider
{
    public interface IPostgreSqlConnectionProviderFactory
    {
        IConnectionProvider<NpgsqlConnection> GetConnectionProvider(string connectionString);
    }
}