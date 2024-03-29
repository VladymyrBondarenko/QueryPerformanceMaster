﻿using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Domain.ExecResults;
using QueryPerformanceMaster.Domain.LoadResults;

namespace QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecutorWithTimeLimit
{
    public class SequentialProfilerExecutorWithTimeLimit : ISequentialProfilerExecutorWithTimeLimit
    {
        private readonly ILoadProfiler _loadProfiler;

        public SequentialProfilerExecutorWithTimeLimit(ILoadProfiler loadProfiler)
        {
            _loadProfiler = loadProfiler;
        }

        public async Task<LoadExecutedResult> ExecuteLoadAsync(string query, int iterationNumber, int timeLimitMiliseconds,
            IProgress<int> queryLoadProgress = null, CancellationToken cancellationToken = default)
        {
            var loadProfilerResult = new List<LoadProfilerResult>();

            for (int i = 1; i <= iterationNumber; i++)
            {
                queryLoadProgress?.Report((i * 100) / iterationNumber);

                var loadResult = await _loadProfiler.ExecuteQueryLoadAsync(query, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    return ProfilerExecuterHelpers.FillLoadExecutedResult(i, loadProfilerResult);
                }

                loadProfilerResult.Add(loadResult);

                var elapsedTime = loadProfilerResult.Sum(x => x.ElapsedTime) + loadResult.ElapsedTime;
                if (elapsedTime >= timeLimitMiliseconds)
                {
                    return ProfilerExecuterHelpers.FillLoadExecutedResult(i, loadProfilerResult);
                }
            }

            return ProfilerExecuterHelpers.FillLoadExecutedResult(iterationNumber, loadProfilerResult);
        }
    }
}
