using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.Messages;
using System.Windows;

namespace MvxStarter.Wpf.Views.ConnectionParamsViews
{
    /// <summary>
    /// Interaction logic for PostgresConnectionParamsView.xaml
    /// </summary>
    public partial class PostgreSqlConnectionParamsView : MvxWpfView
    {
        public PostgreSqlConnectionParamsView()
        {
            InitializeComponent();

            var messanger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _mvxSubscriptionToken = messanger.Subscribe<ConnectionErrorMessage>((e) =>
            {
                MessageBox.Show(e.ErrorMessage, e.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        private MvxSubscriptionToken? _mvxSubscriptionToken;
    }
}
