using MvvmCross.Navigation;
using MvvmCross;
using MvxStarter.Core.Models;
using MvxStarter.Core.ViewModels.ConnectionParamsViewModels;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.SqlProviders;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.ViewModels.Controls;
using QueryPerformanceMaster.Core.ConnectionProvider.PostgreSql;

namespace MvxStarter.Core.Services
{
    internal class SqlProviderManager : ISqlProviderManager 
    { 
        private readonly ISqlProviderServiceFactory _sqlProviderServiceFactory;
        private readonly IMvxNavigationService _navManager;
        private readonly IMsSqlConnectionService _msSqlConnectionService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IPostgreSqlConnectionService _postgresConnectionService;

        public SqlProviderManager(ISqlProviderServiceFactory sqlProviderService, 
            IMvxNavigationService navManager, IMsSqlConnectionService msSqlConnectionService, 
            IMvxMessenger mvxMessenger, IPostgreSqlConnectionService postgresConnectionService)
        {
            _sqlProviderServiceFactory = sqlProviderService;
            _navManager = navManager;
            _msSqlConnectionService = msSqlConnectionService;
            _mvxMessenger = mvxMessenger;
            _postgresConnectionService = postgresConnectionService;
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

        public async Task OpenConnectionParamsView(SqlProvider sqlProvider, string connectionString = null)
        {
            switch (sqlProvider)
            {
                case SqlProvider.SqlServer:

                    var msSqlConnParamsViewModel = new MsSqlConnectionParamsViewModel(this, _msSqlConnectionService, _mvxMessenger, _navManager);

                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        var connectionSetttings = _msSqlConnectionService.GetMsSqlConnectionSettings(connectionString);

                        msSqlConnParamsViewModel.Server = connectionSetttings.Server;
                        msSqlConnParamsViewModel.Authentication = connectionSetttings.IntegratedAuth ? Authentication.IntegratedAuthentication : Authentication.SqlServerAuthentication;
                        msSqlConnParamsViewModel.Login = connectionSetttings.Login;
                        msSqlConnParamsViewModel.IsLoginEnabled = msSqlConnParamsViewModel.Authentication == Authentication.SqlServerAuthentication;
                        msSqlConnParamsViewModel.Password = connectionSetttings.Password;
                        msSqlConnParamsViewModel.IsPasswordEnabled = msSqlConnParamsViewModel.Authentication == Authentication.SqlServerAuthentication;
                        msSqlConnParamsViewModel.ConnectionTimeout = new TemplateNumericUpDown { NumValue = connectionSetttings.ConnectTimeout };
                        msSqlConnParamsViewModel.Pooling = connectionSetttings.EnablePooling;
                    }

                    await _navManager.Navigate(msSqlConnParamsViewModel);
                    break;
                case SqlProvider.PostgreSql:

                    var postgresConnParamsViewModel = new PostgreSqlConnectionParamsViewModel(this, _postgresConnectionService, _mvxMessenger, _navManager);

                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        var connectionSetttings = _postgresConnectionService.GetPostgreSqlConnectionSettings(connectionString);

                        postgresConnParamsViewModel.Server = connectionSetttings.Server;
                        postgresConnParamsViewModel.Login = connectionSetttings.Login;
                        postgresConnParamsViewModel.Port = connectionSetttings.Port;
                        postgresConnParamsViewModel.Password = connectionSetttings.Password;
                        postgresConnParamsViewModel.ConnectionTimeout = new TemplateNumericUpDown { NumValue = connectionSetttings.ConnectTimeout };
                        postgresConnParamsViewModel.Pooling = connectionSetttings.EnablePooling;
                    }

                    await _navManager.Navigate(postgresConnParamsViewModel);
                    break;
            }
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
