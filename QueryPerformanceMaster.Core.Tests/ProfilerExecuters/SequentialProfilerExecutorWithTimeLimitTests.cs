using Moq;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecutorWithTimeLimit;
using QueryPerformanceMaster.Domain.ExecResults;
using QueryPerformanceMaster.Domain.LoadResults;
using Xunit;

namespace QueryPerformanceMaster.Core.UnitTests.ProfilerExecuters
{
    public class SequentialProfilerExecutorWithTimeLimitTests
    {
        private Mock<ILoadProfiler> _loadProfilerMock = new();
        private ISequentialProfilerExecutorWithTimeLimit _sequentialProfilerExecuter;

        public SequentialProfilerExecutorWithTimeLimitTests()
        {
            _sequentialProfilerExecuter = new SequentialProfilerExecutorWithTimeLimit(_loadProfilerMock.Object);
        }

        [Fact]
        public async Task ExecuteLoadAsync_ReturnsElapsedTimeLowerOrEqualThanLimit()
        {
            var cmd = "cmd";
            int iterationNumber = 200;
            int timeLimit = 100;

            _loadProfilerMock
                .Setup(x => x.ExecuteQueryLoadAsync(cmd, CancellationToken.None))
                .ReturnsAsync(new LoadProfilerResult
                {
                    CpuTime = 1,
                    ElapsedTime = 1,
                    ExecTime = TimeSpan.FromMilliseconds(1),
                    LogicalReads = 2
                });

            var loadResult = await _sequentialProfilerExecuter.ExecuteLoadAsync(cmd, iterationNumber, timeLimit);

            Assert.True(loadResult.ElapsedTimeTotal <= timeLimit);
        }

        [Fact]
        public void ExecuteLoadAsync_ReturnsElapsedTimeLowerOrEqualThanLimit_WithCancellation()
        {
            var cmd = "cmd";
            int iterationNumber = 100;
            int timeLimit = int.MaxValue;
            var cts = new CancellationTokenSource();

            _loadProfilerMock
                .Setup(x => x.ExecuteQueryLoadAsync(cmd, cts.Token))
                .ReturnsAsync(new LoadProfilerResult
                {
                    CpuTime = 1,
                    ElapsedTime = 1,
                    ExecTime = TimeSpan.FromMilliseconds(1),
                    LogicalReads = 2
                });
            LoadExecutedResult? loadResult = null;

            var execTask = Task.Run(async () =>
            {
                loadResult = await _sequentialProfilerExecuter.ExecuteLoadAsync(cmd, iterationNumber, timeLimit, cancellationToken: cts.Token);
            });
            var cancelTask = Task.Run(() =>
            {
                cts.Cancel();
            });
            Task.WaitAll(execTask, cancelTask);

            Assert.True(loadResult?.IterationCompleted < iterationNumber);
        }
    }
}
