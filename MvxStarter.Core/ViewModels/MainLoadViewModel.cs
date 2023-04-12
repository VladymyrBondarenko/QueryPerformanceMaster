using AutoMapper;
using MathNet.Numerics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using MvxStarter.Core.Services;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters;

namespace MvxStarter.Core.ViewModels
{
    public class MainLoadViewModel : MvxViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IMvxNavigationService _navManager;

        public MainLoadViewModel(ISqlProviderManager sqlProviderManager,
            IMvxMessenger mvxMessenger, IProfilerExecuterService profilerExecuterService, 
            IConnectionService connectionService, IMvxNavigationService navManager, IMapper mapper)
        {
            SqlProviderViewModel = new SqlProvidersViewModel(sqlProviderManager, mvxMessenger);
            QueryEditorViewModel = new QueryEditorViewModel(mvxMessenger, profilerExecuterService, connectionService, navManager, mapper);
            _mvxMessenger = mvxMessenger;
            _navManager = navManager;
            CloseWindowCommand = new MvxCommand(async () => await CloseWindow());
            CollapseWindowCommand = new MvxCommand(() => CollapseWindow());
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

        public IMvxCommand CloseWindowCommand { get; set; }

        public IMvxCommand CollapseWindowCommand { get; set; }

        public override async Task Initialize()
        {
            await base.Initialize();
            await SqlProviderViewModel.Initialize();
            await QueryEditorViewModel.Initialize();
        }

        public async Task CloseWindow()
        {
            await _navManager.Close(this);
        }

        public void CollapseWindow()
        {
            _mvxMessenger.Publish(new CollapseWindowMessage(this));
        }
    }
}
