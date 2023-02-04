using QueryPerformanceMaster.Core;
using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Services
{
    internal class SqlProviderService : ISqlProviderService
    {
        public List<SqlProviderModel> GetSqlProviders()
        {
            var sqlProviders = Enum.GetNames(typeof(SqlProvider)).ToList();
            return sqlProviders.Select(x => new SqlProviderModel
            {
                SqlProviderTitle = x,
                SqlProviderIcon = getProviderIconPath(Enum.TryParse(typeof(SqlProvider), x, false, out object provider) ? (SqlProvider)provider : SqlProvider.SqlServer)
            }).ToList();
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
