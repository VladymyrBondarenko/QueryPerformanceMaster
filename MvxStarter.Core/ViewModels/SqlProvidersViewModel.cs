using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using MvxStarter.Core.Models;
using MvxStarter.Core.Services;
using System.Collections.ObjectModel;

namespace MvxStarter.Core.ViewModels
{
    public class SqlProvidersViewModel : MvxViewModel
    {
        public SqlProvidersViewModel(ISqlProviderService sqlProviderService,
            IMvxMessenger mvxMessenger)
        {
            _sqlProviderService = sqlProviderService;
            _messageToken = mvxMessenger.Subscribe<LoadedDatabasesMessage>(OnLoadedDatabases);
        }

        private readonly MvxSubscriptionToken _messageToken;

        private readonly ISqlProviderService _sqlProviderService;

        private ObservableCollection<SqlProviderModel> _sqlProviderModels;

        public ObservableCollection<SqlProviderModel> SqlProviderModels
        {
            get { return _sqlProviderModels; }
            set { SetProperty(ref _sqlProviderModels, value); }
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            SqlProviderModels = new ObservableCollection<SqlProviderModel>(_sqlProviderService.GetSqlProviders());
        }

        public void OnLoadedDatabases(LoadedDatabasesMessage message)
        {
            var sqlProvider = SqlProviderModels.FirstOrDefault(x => x.SqlProvider == message.SqlProvider);
            if (sqlProvider != null)
            {
                SqlProviderModels[SqlProviderModels.IndexOf(sqlProvider)] = new SqlProviderModel
                {
                    Name = sqlProvider.Name,
                    SqlProvider = sqlProvider.SqlProvider,
                    IconPath = sqlProvider.IconPath,
                    Databases = new List<SqlProviderDatabaseModel>(
                        message.Databases.Select(x => new SqlProviderDatabaseModel { Name = x })),
                    IsExpanded = sqlProvider.IsExpanded
                };
            }
        }
    }
}
