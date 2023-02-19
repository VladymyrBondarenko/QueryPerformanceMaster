using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using MvxStarter.Core.Services;
using QueryPerformanceMaster.Core.ConnectionProvider.MsSql.ConnectionSettings;
using QueryPerformanceMaster.Domain.SqlProviders;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.Messages;

namespace MvxStarter.Core.ViewModels
{
    public class MsSqlConnectionParamsViewModel : MvxViewModel
    {
        private readonly ISqlProviderService _sqlProviderService;
        private readonly IMsSqlConnectionService _sqlConnectionService;
        private readonly IMvxMessenger _mvxMessenger;

        public MsSqlConnectionParamsViewModel(ISqlProviderService sqlProviderService,
            IMsSqlConnectionService sqlConnectionService, IMvxMessenger mvxMessenger)
        {
            SaveConnectionParamsCommand = new MvxCommand(async () => await SaveConnectionParams());
            Authentication = Authentication.IntegratedAuthentication;
            _sqlProviderService = sqlProviderService;
            _sqlConnectionService = sqlConnectionService;
            _mvxMessenger = mvxMessenger;
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

        public MvvmCross.Commands.IMvxCommand SaveConnectionParamsCommand { get; set; }
        public async Task SaveConnectionParams()
        {
            var connectionString = _sqlConnectionService.GetConnectionString(new MsSqlConnectionSettings
            {
                Server = Server,
                IntegratedAuth = Authentication == Authentication.IntegratedAuthentication,
                Login = Login,
                Password= Password
            });
            var databases = await _sqlProviderService.GetSqlProviderDatabasesAsync(SqlProvider.SqlServer, connectionString);
            _mvxMessenger.Publish(new LoadedDatabasesMessage(this, SqlProvider.SqlServer, databases.Select(x => x.Name).ToList()));

            var navManager = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            await navManager.Close(this);
        }
    }

    public enum Authentication
    {
        IntegratedAuthentication,
        SqlServerAuthentication
    }
}
