using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using MvxStarter.Core.Services;
using QueryPerformanceMaster.Domain.SqlProviders;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.Messages;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.Domain.ConnectionSettings;
using System.Text;
using MvxStarter.Core.ViewModels.Controls;

namespace MvxStarter.Core.ViewModels.ConnectionParamsViewModels
{
    public class MsSqlConnectionParamsViewModel : MvxViewModel
    {
        private readonly ISqlProviderManager _sqlProviderManager;
        private readonly IMsSqlConnectionService _sqlConnectionService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IMvxNavigationService _navManager;

        public MsSqlConnectionParamsViewModel(ISqlProviderManager sqlProviderManager,
            IMsSqlConnectionService sqlConnectionService, IMvxMessenger mvxMessenger,
            IMvxNavigationService navManager)
        {
            SaveConnectionParamsCommand = new MvxCommand(async () => await SaveConnectionParams());
            CloseWindowCommand = new MvxCommand(async () => await CloseWindow());
            Authentication = Authentication.IntegratedAuthentication;
            _sqlProviderManager = sqlProviderManager;
            _sqlConnectionService = sqlConnectionService;
            _mvxMessenger = mvxMessenger;
            _navManager = navManager;
            Server = "(localdb)\\MSSQLLocalDB";
            ConnectionTimeout = new TemplateNumericUpDown { NumValue = 15 };
            Pooling = true;
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

        private TemplateNumericUpDown _connectionTimeout;

        public TemplateNumericUpDown ConnectionTimeout
        {
            get { return _connectionTimeout; }
            set { SetProperty(ref _connectionTimeout, value); }
        }

        private bool _pooling;
        public bool Pooling
        {
            get
            {
                return _pooling;
            }
            set
            {
                SetProperty(ref _pooling, value);
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
                Password = Password,
                EnablePooling = Pooling,
                ConnectTimeout = ConnectionTimeout.NumValue
            };
            var connectionString = _sqlConnectionService.GetConnectionString(connectionSettings);

            var getDatabasesResult = await _sqlProviderManager.GetSqlProviderDatabasesAsync(SqlProvider.SqlServer, connectionString);
            if (getDatabasesResult.Success)
            {
                _mvxMessenger.Publish(new LoadedDatabasesMessage(this, 
                    SqlProvider.SqlServer, getDatabasesResult.SqlProviderDatabases.Select(x => x.Name).ToList(), connectionString));
                await _navManager.Close(this);
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine("Unable to connect to provider, try again.");
                sb.AppendLine($"Error: '{getDatabasesResult.ErrorMessage}'");

                _mvxMessenger.Publish(new ConnectionErrorMessage(this, sb.ToString(), "Connection error"));
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
