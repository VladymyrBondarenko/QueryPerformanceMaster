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
        private readonly MvxSubscriptionToken _addedQueryEditorTabToken;
        private readonly MvxSubscriptionToken _closedQueryEditorTabToken;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IProfilerExecuterService _queryExecuterService;
        private readonly IConnectionService _connectionService;
        private readonly IMvxNavigationService _navManager;
        private readonly IMapper _mapper;

        public QueryEditorViewModel(IMvxMessenger mvxMessenger,
            IProfilerExecuterService queryExecuterService, IConnectionService connectionService,
            IMvxNavigationService navManager, IMapper mapper)
        {
            _addedQueryEditorTabToken = mvxMessenger.Subscribe<AddedQueryEditorTabMessage>(OnAddedQueryEditorTab);
            _closedQueryEditorTabToken = mvxMessenger.Subscribe<ClosedQueryEditorTabMessage>(CloseEditorTab);
            RunQueryCommand = new MvxCommand(async () => await RunQueryAsync());
            _mvxMessenger = mvxMessenger;
            _queryExecuterService = queryExecuterService;
            _connectionService = connectionService;
            _navManager = navManager;
            _mapper = mapper;
            IterationNumber = new TemplateNumericUpDown();
            ThreadNumber = new TemplateNumericUpDown();
            DelayTime = new TemplateNumericUpDown();
            TimeLimit = new TemplateNumericUpDown();
            ProfilerExecuterType = ProfilerExecuterType.SequentialExecutor;
        }

        public IMvxCommand RunQueryCommand { get; set; }


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

        private async Task RunQueryAsync()
        {
            var activeQueryEditorTab = QueryEditorTabs.FirstOrDefault(x => x.IsSelected);
            if(activeQueryEditorTab != null && !string.IsNullOrWhiteSpace(activeQueryEditorTab.QueryEditorContent))
            {
                var results = await _queryExecuterService.ExecuteLoadAsync(ProfilerExecuterType, 
                    new ExecuteLoadParmas 
                    { 
                        ConnectionParams = new SqlConnectionParams
                        {
                            ConnectionString = activeQueryEditorTab.ConnectionString,
                            SqlProvider = activeQueryEditorTab.SqlProvider
                        },
                        Query = activeQueryEditorTab.QueryEditorContent,
                        IterationNumber = IterationNumber.NumValue,
                        ThreadNumber = ThreadNumber.NumValue,
                        DelayMiliseconds = DelayTime.NumValue,
                        TimeLimitMiliseconds = TimeLimit.NumValue
                    });

                if(results != null)
                {
                    var loadResultsViewModel = _mapper.Map<LoadResultsViewModel>(results);
                    await _navManager.Navigate(loadResultsViewModel);
                }
            }
        }

        private void CloseEditorTab(ClosedQueryEditorTabMessage closedQueryEditorTab)
        {
            var activeQueryEditorTab = QueryEditorTabs.FirstOrDefault(x => x == closedQueryEditorTab.EditorTabViewModel);
            QueryEditorTabs.Remove(activeQueryEditorTab);
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
