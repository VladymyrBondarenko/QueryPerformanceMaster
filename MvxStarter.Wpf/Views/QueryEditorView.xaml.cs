using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.Messages;
using System.Windows;

namespace MvxStarter.Wpf.Views
{
    /// <summary>
    /// Interaction logic for QueryEditorView.xaml
    /// </summary>
    public partial class QueryEditorView : MvxWpfView
    {
        public QueryEditorView()
        {
            InitializeComponent();

            var messanger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _mvxSubscriptionToken = messanger.Subscribe<DropBuffersAndCacheErrorMessage>((e) =>
            {
                MessageBox.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        private MvxSubscriptionToken? _mvxSubscriptionToken;
    }
}
