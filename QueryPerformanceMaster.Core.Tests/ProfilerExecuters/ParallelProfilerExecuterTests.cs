using Moq;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Core.ProfilerExecuters.ParallelProfilerExecuter;
using QueryPerformanceMaster.Domain.ExecResults;
using QueryPerformanceMaster.Domain.LoadResults;
using Xunit;

namespace QueryPerformanceMaster.Core.UnitTests.ProfilerExecuters
{
    public class ParallelProfilerExecuterTests
    {
        private Mock<ILoadProfiler> _loadProfilerMock = new();
        private IParallelProfilerExecuter _parallelProfilerExecuter;

        public ParallelProfilerExecuterTests()
        {
            _parallelProfilerExecuter = new ParallelProfilerExecuter(_loadProfilerMock.Object);
        }

        [Fact]
        public async Task ExecuteLoadAsync_ReturnsIterationCompleted()
        {
            var cmd = "cmd";
            int iterationNumber = 10;
            int threadNumber = 10;

            _loadProfilerMock
                .Setup(x => x.ExecuteQueryLoadAsync(cmd, CancellationToken.None))
                .ReturnsAsync(new LoadProfilerResult
                {
                    CpuTime = 1,
                    ElapsedTime = 1,
                    ExecTime = TimeSpan.FromMilliseconds(1),
                    LogicalReads = 2
                });

            var loadResult = await _parallelProfilerExecuter.ExecuteLoadAsync(cmd, threadNumber, iterationNumber);

            Assert.Equal(iterationNumber * threadNumber, loadResult.IterationCompleted);
        }

        [Fact]
        public void ExecuteLoadAsync_ReturnsIterationCompleted_WithCancellation()
        {
            var cmd = "cmd";
            int iterationNumber = 10;
            int threadNumber = 10;
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
                loadResult = await _parallelProfilerExecuter.ExecuteLoadAsync(cmd, threadNumber, iterationNumber, cancellationToken: cts.Token);
            });
            var cancelTask = Task.Run(() =>
            {
                cts.Cancel();
            });
            Task.WaitAll(execTask, cancelTask);

            Assert.True(loadResult?.IterationCompleted < iterationNumber * threadNumber);
        }
    }
}
