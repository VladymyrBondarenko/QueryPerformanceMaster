using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.Messages;
using ScottPlot;
using System.Windows.Input;

namespace MvxStarter.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        private MvxSubscriptionToken? _mvxSubscriptionToken;

        public MainWindow()
        {
            InitializeComponent();

            var messanger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _mvxSubscriptionToken = messanger.Subscribe<CollapseWindowMessage>((e) =>
            {
                WindowState = System.Windows.WindowState.Minimized;
            });
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
    }
}
