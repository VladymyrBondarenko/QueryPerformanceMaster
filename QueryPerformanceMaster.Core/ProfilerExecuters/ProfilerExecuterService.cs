using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters.Factories;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.Domain;
using QueryPerformanceMaster.Domain.ExecResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ProfilerExecuters
{
    public class ProfilerExecuterService : IProfilerExecuterService
    {
        private readonly ILoadProfilersFactory _loadProfilersFactory;
        private readonly ISequentialProfilerExecuterFactory _sequentialExecuterFactory;
        private readonly IParallelProfilerExecuterFactory _profilerExecuterFactory;

        public ProfilerExecuterService(ILoadProfilersFactory loadProfilersFactory,
            ISequentialProfilerExecuterFactory sequentialExecuterFactory,
            IParallelProfilerExecuterFactory profilerExecuterFactory)
        {
            _loadProfilersFactory = loadProfilersFactory;
            _sequentialExecuterFactory = sequentialExecuterFactory;
            _profilerExecuterFactory = profilerExecuterFactory;
        }

        public async Task<LoadExecutedResult> ExecuteSequentialLoadAsync(string query, int iterationNumber, SqlConnectionParams connectionParams,
            CancellationToken cancellationToken = default)
        {
            var loadProfiler = _loadProfilersFactory.GetLoadProfiler(connectionParams);
            var seqExecuter = _sequentialExecuterFactory.GetSequentialProfilerExecuter(loadProfiler);
            return await seqExecuter.ExecuteLoadAsync(query, iterationNumber, cancellationToken);
        }

        public async Task<LoadExecutedResult> ExecuteParallelLoadAsync(string query, int threadNumber, int iterationNumber,
            SqlConnectionParams connectionParams, CancellationToken cancellationToken = default)
        {
            var loadProfiler = _loadProfilersFactory.GetLoadProfiler(connectionParams);
            var parallelExecuter = _profilerExecuterFactory.GetSequentialProfilerExecuter(loadProfiler);
            return await parallelExecuter.ExecuteLoadAsync(query, threadNumber, iterationNumber, cancellationToken);
        }
    }
}
