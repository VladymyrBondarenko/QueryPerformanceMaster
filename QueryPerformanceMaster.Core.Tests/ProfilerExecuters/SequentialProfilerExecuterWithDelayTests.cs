using Moq;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecuterWithDelay;
using QueryPerformanceMaster.Domain.ExecResults;
using QueryPerformanceMaster.Domain.LoadResults;
using Xunit;

namespace QueryPerformanceMaster.Core.UnitTests.ProfilerExecuters
{
    public class SequentialProfilerExecuterWithDelayTests
    {
        private Mock<ILoadProfiler> _loadProfilerMock = new();
        private ISequentialProfilerExecuterWithDelay _sequentialProfilerExecuter;

        public SequentialProfilerExecuterWithDelayTests()
        {
            _sequentialProfilerExecuter = new SequentialProfilerExecuterWithDelay(_loadProfilerMock.Object);
        }

        [Fact]
        public async Task ExecuteLoadAsync_ReturnsIterationCompleted()
        {
            var cmd = "cmd";
            int iterationNumber = 10;
            int delayMiliseconds = 10;

            _loadProfilerMock
                .Setup(x => x.ExecuteQueryLoadAsync(cmd, CancellationToken.None))
                .ReturnsAsync(new LoadProfilerResult
                {
                    CpuTime = 1,
                    ElapsedTime = 1,
                    ExecTime = TimeSpan.FromMilliseconds(100),
                    LogicalReads = 2
                });

            var loadResult = await _sequentialProfilerExecuter.ExecuteLoadAsync(cmd, iterationNumber, delayMiliseconds);

            Assert.True(loadResult?.IterationCompleted == iterationNumber);
        }

        [Fact]
        public void ExecuteLoadAsync_ReturnsIterationCompleted_WithCancellation()
        {
            var cmd = "cmd";
            int iterationNumber = 1000;
            int delayMiliseconds = 100;
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
                loadResult = await _sequentialProfilerExecuter.ExecuteLoadAsync(cmd, iterationNumber, delayMiliseconds, cancellationToken: cts.Token);
            });
            var cancelTask = Task.Run(async () =>
            {
                await Task.Delay(100);
                cts.Cancel();
            });
            Task.WaitAll(execTask, cancelTask);

            Assert.True(loadResult?.IterationCompleted < iterationNumber);
        }
    }
}
