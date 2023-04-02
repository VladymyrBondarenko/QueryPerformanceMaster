using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using MvxStarter.Core.Services;
using MvxStarter.Core.ViewModels.Controls;
using QueryPerformanceMaster.Core.ConnectionProvider.PostgreSql;
using QueryPerformanceMaster.Domain.ConnectionSettings;
using QueryPerformanceMaster.Domain.SqlProviders;
using System.Text;

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
            ConnectionTimeout = new TemplateNumericUpDown { NumValue = 15 };
            Pooling = true;
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
            var connectionSettings = new PostreSqlConnectionSettings
            {
                Server = Server,
                Login = Login,
                Password = Password,
                Port = Port,
                ConnectTimeout = ConnectionTimeout.NumValue,
                EnablePooling = Pooling
            };
            var connectionString = _sqlConnectionService.GetConnectionString(connectionSettings);

            var getDatabasesResult = await _sqlProviderManager.GetSqlProviderDatabasesAsync(SqlProvider, connectionString);
            if (getDatabasesResult.Success)
            {
                _mvxMessenger.Publish(new ConnectedToSqlProviderMessage(this,
                    SqlProvider, getDatabasesResult.SqlProviderDatabases.Select(x => x.Name).ToList(), connectionString));
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
}
