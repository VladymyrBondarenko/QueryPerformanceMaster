using System.Data.SqlClient;

namespace QueryPerformanceMaster.App.Interfaces.ConnectionProvider
{
    public interface IMsSqlConnectionProviderFactory
    {
        IConnectionProvider<SqlConnection> GetConnectionProvider(string connectionString);
    }
}