using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvxStarter.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MsSqlConnectionParamsView.xaml
    /// </summary>
    public partial class MsSqlConnectionParamsView : MvxWpfView
    {
        public MsSqlConnectionParamsView()
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
