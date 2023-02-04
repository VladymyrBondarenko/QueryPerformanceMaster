using MvvmCross.ViewModels;
using MvxStarter.Core.Services;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.ViewModels
{
    public class MainLoadViewModel : MvxViewModel
    {
        public MainLoadViewModel(ISqlProviderService sqlProviderService)
        {
            _sqlProviderService = sqlProviderService;
        }

        private readonly ISqlProviderService _sqlProviderService;

        private List<Models.SqlProviderModel> _sqlProviderModels;

        public List<Models.SqlProviderModel> SqlProviderModels
        {
            get { return _sqlProviderModels; }
            set { SetProperty(ref _sqlProviderModels, value); }
        }

        public override Task Initialize()
        {
            var sqlProviders = _sqlProviderService.GetSqlProviders();
            SqlProviderModels = new List<Models.SqlProviderModel>(
                    sqlProviders.Select(x => new Models.SqlProviderModel { Name = x.SqlProviderTitle, IconPath = x.SqlProviderIcon }));

            return base.Initialize();
        }
    }
}
