using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.Services
{
    public interface ISqlProviderService
    {
        List<SqlProviderModel> GetSqlProviders();
    }
}