using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public string TabTitle { get; set; }

        private string _queryEditorContent;
        public string QueryEditorContent
        {
            get
            {
                return _queryEditorContent;
            }
            set
            {
                SetProperty(ref _queryEditorContent, value);
            }
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
