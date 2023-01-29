using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.Core
{
    public class SqlProviderService : ISqlProviderService
    {
        public List<SqlProviderModel> GetSqlProviders()
        {
            var sqlProviders = Enum.GetNames(typeof(SqlProvider)).ToList();
            return sqlProviders.Select(x => new SqlProviderModel { SqlProviderTitle = x }).ToList();
        }
    }
}
