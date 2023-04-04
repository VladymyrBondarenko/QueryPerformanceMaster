using Moq;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecuter;
using QueryPerformanceMaster.Domain.ExecResults;
using QueryPerformanceMaster.Domain.LoadResults;
using Xunit;

namespace QueryPerformanceMaster.Core.Tests.ProfilerExecuters
{
    public class SequentialProfilerExecuterTests
    {
        private Mock<ILoadProfiler> _loadProfilerMock = new();
        private ISequentialProfilerExecuter _sequentialProfilerExecuter;

        public SequentialProfilerExecuterTests()
        {
            _sequentialProfilerExecuter = new SequentialProfilerExecuter(_loadProfilerMock.Object);
        }

        [Fact]
        public async Task ExecuteLoadAsync_ReturnsIterationCompleted()
        {
            var cmd = "cmd";
            int iterationNumber = 10;

            _loadProfilerMock
                .Setup(x => x.ExecuteQueryLoadAsync(cmd, CancellationToken.None))
                .ReturnsAsync(new LoadProfilerResult 
                { 
                    CpuTime = 1,
                    ElapsedTime = 1,
                    ExecTime = TimeSpan.FromMilliseconds(1),
                    LogicalReads = 2
                });

            var loadResult =  await _sequentialProfilerExecuter.ExecuteLoadAsync(cmd, iterationNumber);

            Assert.Equal(iterationNumber, loadResult.IterationCompleted);
        }

        [Fact]
        public void ExecuteLoadAsync_ReturnsIterationCompleted_WithCancellation()
        {
            var cmd = "cmd";
            int iterationNumber = 1000;
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
                await Task.Delay(100);
                loadResult = await _sequentialProfilerExecuter.ExecuteLoadAsync(cmd, iterationNumber, cancellationToken: cts.Token);
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
