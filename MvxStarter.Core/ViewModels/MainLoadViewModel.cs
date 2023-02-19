using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Services;

namespace MvxStarter.Core.ViewModels
{
    public class MainLoadViewModel : MvxViewModel
    {
        public MainLoadViewModel(ISqlProviderService sqlProviderService,
            IMvxMessenger mvxMessenger)
        {
            _sqlProviderService = sqlProviderService;
            SqlProviderViewModel = new SqlProvidersViewModel(sqlProviderService, mvxMessenger);
        }

        private readonly ISqlProviderService _sqlProviderService;

        private SqlProvidersViewModel _sqlProviderViewModel;

        public SqlProvidersViewModel SqlProviderViewModel
        {
            get { return _sqlProviderViewModel; }
            set { _sqlProviderViewModel = value; }
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await SqlProviderViewModel.Initialize();
        }
    }
}
