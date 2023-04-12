using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using MvxStarter.Core.ViewModels.Controls;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.ViewModels
{
    public class QueryEditorTabViewModel : MvxViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;
        public IMvxCommand CloseEditorTabCommand { get; internal set; }

        public QueryEditorTabViewModel(IMvxMessenger mvxMessenger)
        {
            CloseEditorTabCommand = new MvxCommand(() => CloseEditorTab());
            _mvxMessenger = mvxMessenger;
            QueryEditorControl = new QueryEditorControlViewModel();
        }

        public string TabTitle { get; set; }

        private QueryEditorControlViewModel _queryEditorControl;

        public QueryEditorControlViewModel QueryEditorControl
        {
            get { return _queryEditorControl; }
            set { SetProperty(ref _queryEditorControl, value); }
        }

        public SqlProvider SqlProvider { get; set; }

        public string Database { get; set; }

        public string ConnectionString { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        private void CloseEditorTab()
        {
            _mvxMessenger.Publish(new ClosedQueryEditorTabMessage(this, this));
        }
    }
}
