using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters.Factories;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.ExecResults;

namespace QueryPerformanceMaster.Core.ProfilerExecuters
{
    public class ProfilerExecuterService : IProfilerExecuterService
    {
        private readonly ILoadProfilersFactory _loadProfilersFactory;
        private readonly ISequentialProfilerExecuterFactory _sequentialExecuterFactory;
        private readonly IParallelProfilerExecuterFactory _profilerExecuterFactory;
        private readonly ISequentialProfilerExecuterWithDelayFactory _sequentialExecuterWithDelayFactory;
        private readonly ISequentialProfilerExecutorWithTimeLimitFactory _sequentialExecuterWithTimeLimitFactory;

        public ProfilerExecuterService(ILoadProfilersFactory loadProfilersFactory,
            ISequentialProfilerExecuterFactory sequentialExecuterFactory,
            IParallelProfilerExecuterFactory profilerExecuterFactory,
            ISequentialProfilerExecuterWithDelayFactory profilerExecuterWithDelayFactory,
            ISequentialProfilerExecutorWithTimeLimitFactory sequentialExecuterWithTimeLimitFactory)
        {
            _loadProfilersFactory = loadProfilersFactory;
            _sequentialExecuterFactory = sequentialExecuterFactory;
            _profilerExecuterFactory = profilerExecuterFactory;
            _sequentialExecuterWithDelayFactory = profilerExecuterWithDelayFactory;
            _sequentialExecuterWithTimeLimitFactory = sequentialExecuterWithTimeLimitFactory;
        }

        public async Task<LoadExecutedResult> ExecuteLoadAsync(ProfilerExecuterType executerType, ExecuteLoadParams executeLoadParmas,
            CancellationToken cancellationToken = default)
        {
            LoadExecutedResult res = null;

            var loadProfiler = _loadProfilersFactory.GetLoadProfiler(executeLoadParmas.ConnectionParams);

            switch (executerType)
            {
                case ProfilerExecuterType.ParallerExecutor:
                    var parallelExecuter = _profilerExecuterFactory.GetProfilerExecuter(loadProfiler);
                    res = await parallelExecuter.ExecuteLoadAsync(
                        executeLoadParmas.Query, executeLoadParmas.ThreadNumber, executeLoadParmas.IterationNumber, executeLoadParmas.QueryLoadProgress, cancellationToken);
                    break;
                case ProfilerExecuterType.SequentialExecutor:
                    var seqExecuter = _sequentialExecuterFactory.GetProfilerExecuter(loadProfiler);
                    res = await seqExecuter.ExecuteLoadAsync(executeLoadParmas.Query, executeLoadParmas.IterationNumber, 
                        executeLoadParmas.QueryLoadProgress, cancellationToken);
                    break;
                case ProfilerExecuterType.SequentialExecutorWithDelay:
                    var seqExecuterWithDelay = _sequentialExecuterWithDelayFactory.GetProfilerExecuter(loadProfiler);
                    res = await seqExecuterWithDelay.ExecuteLoadAsync(
                        executeLoadParmas.Query, executeLoadParmas.IterationNumber, executeLoadParmas.DelayMiliseconds, executeLoadParmas.QueryLoadProgress, cancellationToken);
                    break;
                case ProfilerExecuterType.SequentialExecutorWithTimeLimit:
                    var seqExecuterWithTimeLimit = _sequentialExecuterWithTimeLimitFactory.GetProfilerExecuter(loadProfiler);
                    res = await seqExecuterWithTimeLimit.ExecuteLoadAsync(
                        executeLoadParmas.Query, executeLoadParmas.IterationNumber, executeLoadParmas.TimeLimitMiliseconds, executeLoadParmas.QueryLoadProgress, cancellationToken);
                    break;
            }

            return res;
        }
    }
}
