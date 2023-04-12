using AutoMapper;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using MvxStarter.Core.ViewModels.Controls;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.Domain;
using System.Collections.ObjectModel;

namespace MvxStarter.Core.ViewModels
{
    public class QueryEditorViewModel : MvxViewModel
    {
        private IProgress<int> _queryLoadProgress;
        private readonly MvxSubscriptionToken _addedQueryEditorTabToken;
        private readonly MvxSubscriptionToken _closedQueryEditorTabToken;
        private readonly MvxSubscriptionToken _loadedDatabasesToken;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IProfilerExecuterService _queryExecuterService;
        private readonly IConnectionService _connectionService;
        private readonly IMvxNavigationService _navManager;
        private readonly IMapper _mapper;
        private CancellationTokenSource _runLoadCancellationToken;

        public QueryEditorViewModel(IMvxMessenger mvxMessenger,
            IProfilerExecuterService queryExecuterService, IConnectionService connectionService,
            IMvxNavigationService navManager, IMapper mapper)
        {
            // subscribe to events
            _addedQueryEditorTabToken = mvxMessenger.Subscribe<AddedQueryEditorTabMessage>(OnAddedQueryEditorTab);
            _closedQueryEditorTabToken = mvxMessenger.Subscribe<ClosedQueryEditorTabMessage>(CloseEditorTab);
            _loadedDatabasesToken = mvxMessenger.Subscribe<ConnectedToSqlProviderMessage>(OnConnectedToSqlProvider);

            // init worker
            _queryLoadProgress = new Progress<int>(percent =>
            {
                QueryLoadProgress = percent;
            });

            // init commands
            RunQueryCommand = new MvxCommand(async () => await RunQueryAsync());
            CancelQueryLoadCommand = new MvxCommand(() => CancelQueryLoad());

            // init services
            _navManager = navManager;
            _mapper = mapper;
            _mvxMessenger = mvxMessenger;
            _queryExecuterService = queryExecuterService;
            _connectionService = connectionService;

            // init controls
            IterationNumber = new TemplateNumericUpDown();
            ThreadNumber = new TemplateNumericUpDown();
            DelayTime = new TemplateNumericUpDown();
            TimeLimit = new TemplateNumericUpDown();

            ProfilerExecuterType = ProfilerExecuterType.SequentialExecutor;
        }

        public IMvxCommand RunQueryCommand { get; set; }

        public IMvxCommand CancelQueryLoadCommand { get; set; }


        private ObservableCollection<QueryEditorTabViewModel> _queryEditorTabs;

		public ObservableCollection<QueryEditorTabViewModel> QueryEditorTabs
        {
			get { return _queryEditorTabs; }
			set { SetProperty(ref _queryEditorTabs, value); }
		}

        public List<ProfilerExecuterType> ProfilerExecuterTypes
        {
            get
            {
                return Enum.GetValues(typeof(ProfilerExecuterType)).Cast<ProfilerExecuterType>().ToList();
            }
        }

        private ProfilerExecuterType _profilerExecuterType;

        public ProfilerExecuterType ProfilerExecuterType
        {
            get { return _profilerExecuterType; }
            set 
            {
                setInputElementsVisiblity(value);

                SetProperty(ref _profilerExecuterType, value);
            }
        }

        private int _loadProgress;
        public int QueryLoadProgress
        {
            get { return _loadProgress; }
            set { SetProperty(ref _loadProgress, value); }
        }

        private TemplateNumericUpDown _iterationNumber;

        public TemplateNumericUpDown IterationNumber
        {
            get { return _iterationNumber; }
            set { SetProperty(ref _iterationNumber, value); }
        }

        private TemplateNumericUpDown _threadNumber;

        public TemplateNumericUpDown ThreadNumber
        {
            get { return _threadNumber; }
            set { SetProperty(ref _threadNumber, value); }
        }

        private TemplateNumericUpDown _delayTime;

        public TemplateNumericUpDown DelayTime
        {
            get { return _delayTime; }
            set { SetProperty(ref _delayTime, value); }
        }

        private TemplateNumericUpDown _timeLimit;

        public TemplateNumericUpDown TimeLimit
        {
            get { return _timeLimit; }
            set { SetProperty(ref _timeLimit, value); }
        }

        private string _threadNumberVisible;
        public string ThreadNumberVisible
        {
            get { return _threadNumberVisible; }
            set { SetProperty(ref _threadNumberVisible, value); }
        }

        private string _delayTimeVisible;
        public string DelayTimeVisible
        {
            get { return _delayTimeVisible; }
            set { SetProperty(ref _delayTimeVisible, value); }
        }

        private string _timeLimitVisible;
        public string TimeLimitVisible
        {
            get { return _timeLimitVisible; }
            set { SetProperty(ref _timeLimitVisible, value); }
        }

        public void OnAddedQueryEditorTab(AddedQueryEditorTabMessage message)
        {
            var tabTitle = $"{message.SqlProvider}.{message.Database}";
            var count = QueryEditorTabs.Where(x => x.SqlProvider == message.SqlProvider && x.Database == message.Database).Count();
            if(count > 0)
            {
                tabTitle = string.Concat(tabTitle, $" ({count++})");
            }

            QueryEditorTabs.Add(new QueryEditorTabViewModel(_mvxMessenger)
            {
                TabTitle = tabTitle,
                SqlProvider = message.SqlProvider,
                Database = message.Database,
                IsSelected = true,
                ConnectionString = _connectionService.SetDatabaseToConnectionString(message.SqlProvider, message.ConnectionString, message.Database)
            });
        }

        public override Task Initialize()
        {
            QueryEditorTabs = new ObservableCollection<QueryEditorTabViewModel>();

            return base.Initialize();
        }

        private void OnConnectedToSqlProvider(ConnectedToSqlProviderMessage message)
        {
            var editorTabsByProvider = QueryEditorTabs.Where(x => x.SqlProvider == message.SqlProvider).ToList();

            foreach (var editorTab in editorTabsByProvider)
            {
                // close tabs with databases that are not longer existing after save connection params
                if (!message.Databases.Contains(editorTab.Database))
                {
                    CloseEditorTab(editorTab);
                    continue;
                }

                // otherwise - apply new connection string to tab
                editorTab.ConnectionString = _connectionService.SetDatabaseToConnectionString(message.SqlProvider, message.ConnectionString, editorTab.Database);
            }
        }

        private async Task RunQueryAsync()
        {
            var activeQueryEditorTab = QueryEditorTabs.FirstOrDefault(x => x.IsSelected);
            if(activeQueryEditorTab != null && !string.IsNullOrWhiteSpace(activeQueryEditorTab.QueryEditorControl.QueryEditorContent))
            {
                var connectionString = activeQueryEditorTab.ConnectionString;
                if (ThreadNumber.NumValue > 0)
                {
                    // set pool size as threads number
                    connectionString = _connectionService.SetPoolSizeToConnectionString(activeQueryEditorTab.SqlProvider, connectionString, ThreadNumber.NumValue);
                }

                _runLoadCancellationToken = new CancellationTokenSource();

                var results = await _queryExecuterService.ExecuteLoadAsync(ProfilerExecuterType, 
                    new ExecuteLoadParams 
                    { 
                        ConnectionParams = new SqlConnectionParams
                        {
                            ConnectionString = connectionString,
                            SqlProvider = activeQueryEditorTab.SqlProvider
                        },
                        Query = activeQueryEditorTab.QueryEditorControl.QueryEditorContent,
                        IterationNumber = IterationNumber.NumValue,
                        ThreadNumber = ThreadNumber.NumValue,
                        DelayMiliseconds = DelayTime.NumValue,
                        TimeLimitMiliseconds = TimeLimit.NumValue,
                        QueryLoadProgress = _queryLoadProgress
                    }, _runLoadCancellationToken.Token);

                if(results != null)
                {
                    var loadResultsViewModel = _mapper.Map<LoadResultsViewModel>(results);
                    await _navManager.Navigate(loadResultsViewModel);
                }

                QueryLoadProgress = 0;
            }
        }

        private void CancelQueryLoad()
        {
            _runLoadCancellationToken?.Cancel();
        }

        private void CloseEditorTab(ClosedQueryEditorTabMessage closedQueryEditorTab)
        {
            var activeQueryEditorTab = QueryEditorTabs.FirstOrDefault(x => x == closedQueryEditorTab.EditorTabViewModel);
            CloseEditorTab(activeQueryEditorTab);
        }

        private void CloseEditorTab(QueryEditorTabViewModel editorTabViewModel)
        {
            QueryEditorTabs.Remove(editorTabViewModel);
        }

        private void setInputElementsVisiblity(ProfilerExecuterType profilerExecuterType)
        {
            switch (profilerExecuterType)
            {
                case ProfilerExecuterType.ParallerExecutor:
                    // set thread number visibility
                    ThreadNumberVisible = "Visible";
                    ThreadNumber.ControlVisible = "Visible";

                    // set delay time visibility
                    DelayTimeVisible = "Hidden";
                    DelayTime.ControlVisible = "Hidden";

                    // set time limit visibility
                    TimeLimitVisible = "Hidden";
                    TimeLimit.ControlVisible = "Hidden";
                    break;
                case ProfilerExecuterType.SequentialExecutor:
                    // set thread number visibility
                    ThreadNumberVisible = "Hidden";
                    ThreadNumber.ControlVisible = "Hidden";

                    //set delay time visibility
                    DelayTimeVisible = "Hidden";
                    DelayTime.ControlVisible = "Hidden";

                    // set time limit visibility
                    TimeLimitVisible = "Hidden";
                    TimeLimit.ControlVisible = "Hidden";
                    break;
                case ProfilerExecuterType.SequentialExecutorWithDelay:
                    // set thread number visibility
                    ThreadNumberVisible = "Hidden";
                    ThreadNumber.ControlVisible = "Hidden";

                    // set time limit visibility
                    TimeLimitVisible = "Hidden";
                    TimeLimit.ControlVisible = "Hidden";

                    //set delay time visibility
                    DelayTimeVisible = "Visible";
                    DelayTime.ControlVisible = "Visible";
                    break;
                case ProfilerExecuterType.SequentialExecutorWithTimeLimit:
                    //set delay time visibility
                    DelayTimeVisible = "Hidden";
                    DelayTime.ControlVisible = "Hidden";

                    // set thread number visibility
                    ThreadNumberVisible = "Hidden";
                    ThreadNumber.ControlVisible = "Hidden";

                    // set time limit visibility
                    TimeLimitVisible = "Visible";
                    TimeLimit.ControlVisible = "Visible";
                    break;
                default:
                    break;
            }
        }
    }
}
