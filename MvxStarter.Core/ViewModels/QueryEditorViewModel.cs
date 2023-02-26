using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using MvxStarter.Core.Models;
using MvxStarter.Core.Services;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.ViewModels
{
    public class QueryEditorViewModel : MvxViewModel
    {
        private readonly MvxSubscriptionToken _addedQueryEditorTabToken;
        private readonly IProfilerExecuterService _queryExecuterService;
        private readonly IConnectionService _connectionService;

        public QueryEditorViewModel(IMvxMessenger mvxMessenger,
            IProfilerExecuterService queryExecuterService, IConnectionService connectionService)
        {
            _addedQueryEditorTabToken = mvxMessenger.Subscribe<AddedQueryEditorTabMessage>(OnAddedQueryEditorTab);
            RunQueryCommand = new MvxCommand(async () => await RunQueryAsync());
            _queryExecuterService = queryExecuterService;
            _connectionService = connectionService;
        }

        public IMvxCommand RunQueryCommand { get; set; }

        private ObservableCollection<QueryEditorTabModel> _queryEditorTabs;

		public ObservableCollection<QueryEditorTabModel> QueryEditorTabs
        {
			get { return _queryEditorTabs; }
			set { SetProperty(ref _queryEditorTabs, value); }
		}

        public void OnAddedQueryEditorTab(AddedQueryEditorTabMessage message)
        {
            var tabTitle = $"{message.SqlProvider}.{message.Database}";
            var count = QueryEditorTabs.Where(x => x.SqlProvider == message.SqlProvider && x.Database == message.Database).Count();
            if(count > 0)
            {
                tabTitle = string.Concat(tabTitle, $" ({count++})");
            }

            QueryEditorTabs.Add(new QueryEditorTabModel 
            {
                TabTitle = tabTitle,
                SqlProvider = message.SqlProvider,
                Database = message.Database,
                IsSelected = true,
                ConnectionString = _connectionService.SetDatabaseToConnectionString(message.SqlProvider, message.ConnectionString, message.Database)
            });
        }

        public override Task Initialize()
        {
            QueryEditorTabs = new ObservableCollection<QueryEditorTabModel>();

            return base.Initialize();
        }

        private async Task RunQueryAsync()
        {
            var activeQueryEditorTab = QueryEditorTabs.FirstOrDefault(x => x.IsSelected);
            if(activeQueryEditorTab != null && !string.IsNullOrWhiteSpace(activeQueryEditorTab.QueryEditorContent))
            {
                var results = await _queryExecuterService.ExecuteSequentialLoadAsync(
                    activeQueryEditorTab.QueryEditorContent, 20, 
                    new SqlConnectionParams 
                    { 
                        ConnectionString = activeQueryEditorTab.ConnectionString,
                        SqlProvider = activeQueryEditorTab.SqlProvider
                    });
            }
        }
    }
}
