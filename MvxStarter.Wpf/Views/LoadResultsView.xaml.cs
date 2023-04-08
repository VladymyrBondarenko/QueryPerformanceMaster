using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.Messages;
using ScottPlot;
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
    /// Interaction logic for LoadResultsView.xaml
    /// </summary>
    public partial class LoadResultsView : MvxWpfView
    {
        private MvxSubscriptionToken _elapsedTimeToken;
        private MvxSubscriptionToken _cpuTimeToken;
        private MvxSubscriptionToken _logicalReadsToken;

        public LoadResultsView()
        {
            InitializeComponent();

            ElapsedTimePlot.Plot.Style(ScottPlot.Style.Blue2);
            ElapsedTimePlot.Refresh();

            CpuTimePlot.Plot.Style(ScottPlot.Style.Blue2);
            CpuTimePlot.Refresh();

            LogicalReadsPlot.Plot.Style(ScottPlot.Style.Blue2);
            LogicalReadsPlot.Refresh();

            var messanger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _elapsedTimeToken = messanger.Subscribe<InitElapsedTimePlotMessage>(setElapsedTimePlot);
            _cpuTimeToken = messanger.Subscribe<InitCpuTimePlotMessage>(setCpuTimePlot);
            _logicalReadsToken = messanger.Subscribe<InitLogicalReadsPlotMessage>(setLogicalReadsPlot);
        }

        private void setElapsedTimePlot(InitElapsedTimePlotMessage e)
        {
            ElapsedTimePlot.Plot.AddScatter(e.DataX, e.DataY);
            ElapsedTimePlot.Refresh();
        }

        private void setCpuTimePlot(InitCpuTimePlotMessage e)
        {
            CpuTimePlot.Plot.AddScatter(e.DataX, e.DataY);
            CpuTimePlot.Refresh();
        }

        private void setLogicalReadsPlot(InitLogicalReadsPlotMessage e)
        {
            LogicalReadsPlot.Plot.AddScatter(e.DataX, e.DataY);
            LogicalReadsPlot.Refresh();
        }
    }
}
