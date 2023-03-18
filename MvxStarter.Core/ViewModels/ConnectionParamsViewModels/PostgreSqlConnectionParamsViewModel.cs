using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using MvxStarter.Core.Services;
using QueryPerformanceMaster.Core.ConnectionProvider.PostgreSql;
using QueryPerformanceMaster.Domain.ConnectionSettings;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.ViewModels.ConnectionParamsViewModels
{
    public class PostgreSqlConnectionParamsViewModel : MvxViewModel
    {
        private readonly ISqlProviderManager _sqlProviderManager;
        private readonly IPostgreSqlConnectionService _sqlConnectionService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IMvxNavigationService _navManager;

        public PostgreSqlConnectionParamsViewModel(ISqlProviderManager sqlProviderManager,
            IPostgreSqlConnectionService sqlConnectionService, IMvxMessenger mvxMessenger,
            IMvxNavigationService navManager)
        {
            SaveConnectionParamsCommand = new MvxCommand(async () => await SaveConnectionParams());
            CloseWindowCommand = new MvxCommand(async () => await CloseWindow());
            _sqlProviderManager = sqlProviderManager;
            _sqlConnectionService = sqlConnectionService;
            _mvxMessenger = mvxMessenger;
            _navManager = navManager;
            Server = "127.0.0.1";
        }

        private SqlProvider SqlProvider => SqlProvider.PostgreSql;

        private string _server;
        public string Server
        {
            get { return _server; }
            set
            {
                SetProperty(ref _server, value);
            }
        }

        private int _port;
        public int Port
        {
            get { return _port; }
            set
            {
                SetProperty(ref _port, value);
            }
        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                SetProperty(ref _login, value);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
            }
        }

        private string _defaultDatabase;

        public string DefaultDatabase
        {
            get { return _defaultDatabase; }
            set { SetProperty(ref _defaultDatabase, value); }
        }

        private List<string> _databases;

        public List<string> Databases
        {
            get { return _databases; }
            set { SetProperty(ref _databases, value); }
        }

        private bool _isDatabasesComboBoxOpen;
        public bool IsDatabasesComboBoxOpen
        {
            get { return _isDatabasesComboBoxOpen; }
            set
            {
                SetProperty(ref _isDatabasesComboBoxOpen, value);
            }
        }

        public IMvxCommand SaveConnectionParamsCommand { get; set; }

        public IMvxCommand CloseWindowCommand { get; set; }

        public async Task SaveConnectionParams()
        {
            var connectionSettings = new PostreSqlConnectionSettings
            {
                Server = Server,
                Login = Login,
                Password = Password
            };
            var connectionString = _sqlConnectionService.GetConnectionString(connectionSettings);

            var getDatabasesResult = await _sqlProviderManager.GetSqlProviderDatabasesAsync(SqlProvider, connectionString);
            if (getDatabasesResult.Success)
            {
                _mvxMessenger.Publish(new LoadedDatabasesMessage(this,
                    SqlProvider, getDatabasesResult.SqlProviderDatabases.Select(x => x.Name).ToList(), connectionString));
                await _navManager.Close(this);
            }
            else
            {
                // TODO: error message
            }
        }

        public async Task CloseWindow()
        {
            await _navManager.Close(this);
        }
    }
}
