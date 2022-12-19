using SqlQueryPerformanceProfiler.Executers.ExecResults;
using SqlQueryPerformanceProfiler.Executers.Interfaces;
using SqlQueryPerformanceProfiler.Profilers;
using SqlQueryPerformanceProfiler.Profilers.Interfaces;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Executers
{
    internal class SequentialProfilerExecuterWithDelay : IProfilerExecuter
    {
        private readonly LoadProfilerParams _sqlQueryLoadParams;
        private readonly ILoadProfiler _loadProfiler;

        public SequentialProfilerExecuterWithDelay(LoadProfilerParams sqlQueryLoadSettings,
            ILoadProfiler loadProfiler)
        {
            _sqlQueryLoadParams = sqlQueryLoadSettings;
            _loadProfiler = loadProfiler;
        }

        public async Task<LoadExecutedResult> ExecuteLoadAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_sqlQueryLoadParams.Query) ||
                string.IsNullOrWhiteSpace(_sqlQueryLoadParams.ConnectionString))
            {
                throw new ArgumentException("Invalid query load params!");
            }

            var loadProfilerResult = new List<LoadProfilerResult>();

            for (int i = 1; i <= _sqlQueryLoadParams.IterationsNumber; i++)
            {
                await Task.Delay(_sqlQueryLoadParams.DelayMiliseconds);
                var loadResult = await _loadProfiler.ExecuteQueryLoadAsync(cancellationToken);
                loadProfilerResult.Add(loadResult);
            }

            return ProfilerExecuterHelpers.FillLoadExecutedResult(_sqlQueryLoadParams, loadProfilerResult);
        }
    }
}
