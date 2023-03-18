using QueryPerformanceMaster.Domain.ConnectionSettings;

namespace QueryPerformanceMaster.Core.ConnectionProvider.PostgreSql
{
    public interface IPostgreSqlConnectionService
    {
        string GetConnectionString(PostreSqlConnectionSettings settings);
        PostreSqlConnectionSettings GetPostgreSqlConnectionSettings(string connectionString);
    }
}