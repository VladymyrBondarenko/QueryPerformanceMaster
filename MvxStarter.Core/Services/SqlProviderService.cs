using MvxStarter.Core.Models;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.Services
{
    internal class SqlProviderManager : ISqlProviderManager 
    { 
        private readonly ISqlProviderServiceFactory _sqlProviderServiceFactory;

        public SqlProviderManager(ISqlProviderServiceFactory sqlProviderService)
        {
            _sqlProviderServiceFactory = sqlProviderService;
        }

        public List<SqlProviderModel> GetSqlProviders()
        {
            var sqlProviders = Enum.GetNames(typeof(SqlProvider)).ToList();
            return sqlProviders.Select(x => 
            {
                var sqlProvider = Enum.TryParse(typeof(SqlProvider), x, false, out object provider) ? (SqlProvider)provider : SqlProvider.SqlServer;
                var sqlProviderModel = new SqlProviderModel
                {
                    Name = x,
                    SqlProvider = sqlProvider,
                    IconPath = getProviderIconPath(sqlProvider),
                    Databases = new List<SqlProviderDatabaseModel> { new SqlProviderDatabaseModel { Name = string.Empty } }
                };
                return sqlProviderModel;
            }).ToList();
        }

        public async Task<GetProviderDatabasesResult> GetSqlProviderDatabasesAsync(SqlProvider sqlProvider, string connectionString)
        {
            var sqlProviderManager = _sqlProviderServiceFactory.GetSqlProviderService(new SqlConnectionParams
            {
                SqlProvider = sqlProvider,
                ConnectionString = connectionString
            });
            return await sqlProviderManager.GetSqlProviderDatabasesAsync();
        }

        private string getProviderIconPath(SqlProvider sqlProvider)
        {
            var iconPath = string.Empty;
            switch (sqlProvider)
            {
                case SqlProvider.PostgreSql:
                    iconPath = "pack://application:,,,/wwwroot/Icons/PostgresIcon.png";
                    break;
                case SqlProvider.SqlServer:
                    iconPath = "pack://application:,,,/wwwroot/Icons/SqlServerIcon.png";
                    break;
                case SqlProvider.Oracle:
                    iconPath = "pack://application:,,,/wwwroot/Icons/OracleIcon.png";
                    break;
            }
            return iconPath;
        }
    }
}
