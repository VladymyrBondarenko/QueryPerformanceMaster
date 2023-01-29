using QueryPerformanceMaster.Domain.SqlProviders;

namespace QueryPerformanceMaster.Core
{
    public interface ISqlProviderService
    {
        List<SqlProviderModel> GetSqlProviders();
    }
}