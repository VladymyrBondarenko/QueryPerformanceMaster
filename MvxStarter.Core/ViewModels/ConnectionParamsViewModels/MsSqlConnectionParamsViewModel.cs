using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using MvxStarter.Core.Services;
using QueryPerformanceMaster.Domain.SqlProviders;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.Messages;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.Domain.ConnectionSettings;

namespace MvxStarter.Core.ViewModels.ConnectionParamsViewModels
{
    public class MsSqlConnectionParamsViewModel : MvxViewModel
    {
        private readonly ISqlProviderService _sqlProviderService;
        private readonly IMsSqlConnectionService _sqlConnectionService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IMvxNavigationService _navManager;

        public MsSqlConnectionParamsViewModel(ISqlProviderService sqlProviderService,
            IMsSqlConnectionService sqlConnectionService, IMvxMessenger mvxMessenger,
            IMvxNavigationService navManager)
        {
            SaveConnectionParamsCommand = new MvxCommand(async () => await SaveConnectionParams());
            CloseWindowCommand = new MvxCommand(async () => await CloseWindow());
            Authentication = Authentication.IntegratedAuthentication;
            _sqlProviderService = sqlProviderService;
            _sqlConnectionService = sqlConnectionService;
            _mvxMessenger = mvxMessenger;
            _navManager = navManager;
            Server = "(localdb)\\MSSQLLocalDB";
        }

        private string _server;
        public string Server
        {
            get { return _server; }
            set
            {
                SetProperty(ref _server, value);
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

        private bool _isLoginEnabled;
        public bool IsLoginEnabled
        {
            get
            {
                return _isLoginEnabled;
            }
            set
            {
                SetProperty(ref _isLoginEnabled, value);
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

        private bool _isPasswordEnabled;
        public bool IsPasswordEnabled
        {
            get
            {
                return _isPasswordEnabled;
            }
            set
            {
                SetProperty(ref _isPasswordEnabled, value);
            }
        }

        private Authentication _authentication;
        public Authentication Authentication
        {
            get { return _authentication; }
            set
            {
                IsLoginEnabled = IsPasswordEnabled = value == Authentication.SqlServerAuthentication;
                SetProperty(ref _authentication, value);
            }
        }

        public List<Authentication> Authentications
        {
            get
            {
                return Enum.GetValues(typeof(Authentication)).Cast<Authentication>().ToList();
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
            var connectionSettings = new MsSqlConnectionSettings
            {
                Server = Server,
                IntegratedAuth = Authentication == Authentication.IntegratedAuthentication,
                Login = Login,
                Password = Password
            };
            var connectionString = _sqlConnectionService.GetConnectionString(connectionSettings);

            var getDatabasesResult = await _sqlProviderService.GetSqlProviderDatabasesAsync(SqlProvider.SqlServer, connectionString);
            if (getDatabasesResult.Success)
            {
                _mvxMessenger.Publish(new LoadedDatabasesMessage(this, 
                    SqlProvider.SqlServer, getDatabasesResult.SqlProviderDatabases.Select(x => x.Name).ToList(), connectionString));
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

    public enum Authentication
    {
        IntegratedAuthentication,
        SqlServerAuthentication
    }
}
