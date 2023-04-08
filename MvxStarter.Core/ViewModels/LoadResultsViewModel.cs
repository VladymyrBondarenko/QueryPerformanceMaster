using MathNet.Numerics;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using QueryPerformanceMaster.Domain.LoadResults;

namespace MvxStarter.Core.ViewModels
{
    public class LoadResultsViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navManager;

        public LoadResultsViewModel()
        {
            CloseWindowCommand = new MvxCommand(async () => await CloseWindow());
            ViewErrorsCommand = new MvxCommand(async () => await ViewErrors());
            _navManager = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
        }

        private int _tabControlSelectedIndex;

        public int TabControlSelectedIndex
        {
            get { return _tabControlSelectedIndex; }
            set 
            { 
                if(value == 1)
                {
                    InitElapsedTimePlot();
                    InitCpuTimePlot();
                    InitLogicalReadsPlot();
                }

                _tabControlSelectedIndex = value; 
            }
        }

        private List<double> _elapsedTimes;
        public List<double> ElapsedTimes 
        {
            get => _elapsedTimes;
            set => SetProperty(ref _elapsedTimes, value);
        }

        private List<double> _cpuTimes;
        public List<double> CpuTimes
        {
            get => _cpuTimes;
            set => SetProperty(ref _cpuTimes, value);
        }

        private List<double> _logicalReads;
        public List<double> LogicalReads
        {
            get => _logicalReads;
            set => SetProperty(ref _logicalReads, value);
        }

        private decimal _cpuTimeTotal;
        public decimal CpuTimeTotal { get => _cpuTimeTotal; set => SetProperty(ref _cpuTimeTotal, value); }

        private decimal _cpuTimeAvg;
        public decimal CpuTimeAvg { get => _cpuTimeAvg; set => SetProperty(ref _cpuTimeAvg, value); }

        private decimal _cpuTimeMod;
        public decimal CpuTimeMod { get => _cpuTimeMod; set => SetProperty(ref _cpuTimeMod, value); }

        private decimal _cpuTimeStdDev;
        public decimal CpuTimeStdDev { get => _cpuTimeStdDev; set => SetProperty(ref _cpuTimeStdDev, value); }

        private decimal _logicalReadsTotal;
        public decimal LogicalReadsTotal { get => _logicalReadsTotal; set => SetProperty(ref _logicalReadsTotal, value); }

        private decimal _logicalReadsAvg;
        public decimal LogicalReadsAvg { get => _logicalReadsAvg; set => SetProperty(ref _logicalReadsAvg, value); }

        private decimal _logicalReadsMod;
        public decimal LogicalReadsMod { get => _logicalReadsMod; set => SetProperty(ref _logicalReadsMod, value); }

        private decimal _logicalReadsStdDev;
        public decimal LogicalReadsStdDev { get => _logicalReadsStdDev; set => SetProperty(ref _logicalReadsStdDev, value); }

        private decimal _elapsedTimeTotal;
        public decimal ElapsedTimeTotal { get => _elapsedTimeTotal; set => SetProperty(ref _elapsedTimeTotal, value); }

        private decimal _elapsedTimeAvg;
        public decimal ElapsedTimeAvg { get => _elapsedTimeAvg; set => SetProperty(ref _elapsedTimeAvg, value); }

        private decimal _elapsedTimeMod;
        public decimal ElapsedTimeMod { get => _elapsedTimeMod; set => SetProperty(ref _elapsedTimeMod, value); }

        private decimal _elapsedTimeStdDev;
        public decimal ElapsedTimeStdDev { get => _elapsedTimeStdDev; set => SetProperty(ref _elapsedTimeStdDev, value); }

        private TimeSpan _execTime;
        public TimeSpan ExecTime { get => _execTime; set => SetProperty(ref _execTime, value); }

        private int _iterationCompleted;
        public int IterationCompleted { get => _iterationCompleted; set => SetProperty(ref _iterationCompleted, value); }

        public int ErrorNumber { get => SqlQueryLoadErrors.Sum(x => x.Count); }

        private List<LoadProfilerError> _sqlQueryLoadErrors = new List<LoadProfilerError>();
        public List<LoadProfilerError> SqlQueryLoadErrors { get => _sqlQueryLoadErrors; set => SetProperty(ref _sqlQueryLoadErrors, value); }

        public IMvxCommand CloseWindowCommand { get; set; }
        public async Task CloseWindow()
        {
            await _navManager.Close(this);
        }

        public IMvxCommand ViewErrorsCommand { get; set; }
        public async Task ViewErrors()
        {
            if(SqlQueryLoadErrors.Any())
            {
                await _navManager.Navigate(new LoadErrorsViewModel { SqlQueryLoadErrors = SqlQueryLoadErrors });
            }
        }

        public void InitElapsedTimePlot()
        {
            var mvxMessenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            mvxMessenger.Publish(new InitElapsedTimePlotMessage(this,
                Enumerable.Range(1, IterationCompleted).Select(x => Convert.ToDouble(x)).ToArray(), ElapsedTimes.ToArray()));
        }

        public void InitCpuTimePlot()
        {
            var mvxMessenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            mvxMessenger.Publish(new InitCpuTimePlotMessage(this,
                Enumerable.Range(1, IterationCompleted).Select(x => Convert.ToDouble(x)).ToArray(), CpuTimes.ToArray()));
        }

        public void InitLogicalReadsPlot()
        {
            var mvxMessenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            mvxMessenger.Publish(new InitLogicalReadsPlotMessage(this,
                Enumerable.Range(1, IterationCompleted).Select(x => Convert.ToDouble(x)).ToArray(), LogicalReads.ToArray()));
        }
    }
}
