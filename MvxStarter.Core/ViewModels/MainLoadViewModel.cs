﻿using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Services;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters;

namespace MvxStarter.Core.ViewModels
{
    public class MainLoadViewModel : MvxViewModel
    {
        public MainLoadViewModel(ISqlProviderService sqlProviderService,
            IMvxMessenger mvxMessenger, IProfilerExecuterService profilerExecuterService, 
            IConnectionService connectionService)
        {
            SqlProviderViewModel = new SqlProvidersViewModel(sqlProviderService, mvxMessenger);
            QueryEditorViewModel = new QueryEditorViewModel(mvxMessenger, profilerExecuterService, connectionService);
        }

        private SqlProvidersViewModel _sqlProviderViewModel;

        public SqlProvidersViewModel SqlProviderViewModel
        {
            get { return _sqlProviderViewModel; }
            set { _sqlProviderViewModel = value; }
        }

        private QueryEditorViewModel _queryEditorViewModel;

        public QueryEditorViewModel QueryEditorViewModel
        {
            get { return _queryEditorViewModel; }
            set { _queryEditorViewModel = value; }
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await SqlProviderViewModel.Initialize();
            await QueryEditorViewModel.Initialize();
        }
    }
}
