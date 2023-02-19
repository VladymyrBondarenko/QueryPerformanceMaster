using MvxStarter.Core.Models;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Core;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Services
{
    internal class SqlProviderService : ISqlProviderService
    {
        private readonly ISqlProviderManagerFactory _sqlProviderManagerFactory;

        public SqlProviderService(ISqlProviderManagerFactory sqlProviderManager)
        {
            _sqlProviderManagerFactory = sqlProviderManager;
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

        public async Task<List<SqlProviderDatabase>> GetSqlProviderDatabasesAsync(SqlProvider sqlProvider, string connectionString)
        {
            var sqlProviderManager = _sqlProviderManagerFactory.GetSqlProviderService(new SqlConnectionParams
            {
                SqlProvider= sqlProvider,
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
