using MvvmCross.ViewModels;

namespace MvxStarter.Core.ViewModels.Controls
{
    public class QueryEditorControlViewModel : MvxViewModel
    {
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
    }
}
