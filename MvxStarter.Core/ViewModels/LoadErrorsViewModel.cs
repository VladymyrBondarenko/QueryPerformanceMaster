using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using QueryPerformanceMaster.Domain.LoadResults;

namespace MvxStarter.Core.ViewModels
{
    /// <summary>
    /// TODO: binding doesn't for with DataGridTextColumn,
    /// figure out how to display errors
    /// https://github.com/MvvmCross/MvvmCross/issues/3660
    /// </summary>
    public class LoadErrorsViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navManager;

        public LoadErrorsViewModel()
        {
            CloseWindowCommand = new MvxCommand(async () => await CloseWindow());
            _navManager = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
        }

        private List<LoadProfilerError> _sqlQueryLoadErrors;
        public List<LoadProfilerError> SqlQueryLoadErrors { get => _sqlQueryLoadErrors; set => SetProperty(ref _sqlQueryLoadErrors, value); }

        public IMvxCommand CloseWindowCommand { get; set; }
        public async Task CloseWindow()
        {
            await _navManager.Close(this);
        }
    }
}
