using MathNet.Numerics.Statistics;
using QueryPerformanceMaster.Domain.ExecResults;
using QueryPerformanceMaster.Domain.LoadResults;

namespace QueryPerformanceMaster.Core.ProfilerExecuters
{
    internal class ProfilerExecuterHelpers
    {
        public static LoadExecutedResult FillLoadExecutedResult(int iterationNumber,
            List<LoadProfilerResult> loadProfilerResults)
        {
            var res = new LoadExecutedResult();

            if(iterationNumber == 0)
            {
                return res;
            }

            var cpuTimes = loadProfilerResults.Select(x => x.CpuTime).ToList();
            var logicalReads = loadProfilerResults.Select(x => x.LogicalReads).ToList();
            var elapsedTimes = loadProfilerResults.Select(x => x.ElapsedTime).ToList();
            var execTimes = loadProfilerResults.Select(x => x.ExecTime).ToList();

            //calc total
            res.CpuTimeTotal = Math.Round((decimal)cpuTimes.Sum() / 1000m, 4);
            res.LogicalReadsTotal = Math.Round((decimal)logicalReads.Sum(), 4);
            res.ElapsedTimeTotal = Math.Round((decimal)elapsedTimes.Sum() / 1000m, 4);

            foreach (var result in loadProfilerResults)
            {
                res.ExecTime += result.ExecTime;
            }

            // calc avg
            res.CpuTimeAvg = Math.Round(res.CpuTimeTotal / iterationNumber, 4);
            res.LogicalReadsAvg = Math.Round(res.LogicalReadsTotal / iterationNumber, 4);
            res.ElapsedTimeAvg = Math.Round(res.ElapsedTimeTotal / iterationNumber, 4);
            res.ExecTimeAvg = res.ExecTime / iterationNumber;

            // calc mod
            var execTimeMiliseconds = execTimes.Select(x => x.TotalMilliseconds).ToList();
            res.CpuTimeMod = Math.Round((decimal)cpuTimes.Median(), 4);
            res.LogicalReadsMod = Math.Round((decimal)logicalReads.Median(), 4);
            res.ElapsedTimeMod = Math.Round((decimal)elapsedTimes.Median(), 4);
            res.ExecTimeMod = TimeSpan.FromMilliseconds(execTimeMiliseconds.Median());

            // calc standard dev
            res.CpuTimeStdDev = cpuTimes.Count > 1 ? Math.Round((decimal)cpuTimes.StandardDeviation(), 4) : 0;
            res.LogicalReadsStdDev = logicalReads.Count > 1 ? Math.Round((decimal)logicalReads.StandardDeviation(), 4) : 0;
            res.ElapsedTimeStdDev = elapsedTimes.Count > 1 ? Math.Round((decimal)elapsedTimes.StandardDeviation(), 4) : 0;
            res.ExecTimeStdDev = execTimeMiliseconds.Count > 1 ? 
                TimeSpan.FromMilliseconds(execTimeMiliseconds.StandardDeviation()) : TimeSpan.FromMilliseconds(0);

            // calc errors
            var groupedErrors = loadProfilerResults
                .Where(x => !string.IsNullOrEmpty(x.SqlQueryLoadError))
                .Select(x => x.SqlQueryLoadError)
                .GroupBy(x => x);

            foreach (var error in groupedErrors)
            {
                res.SqlQueryLoadErrors.Add(new LoadProfilerError
                {
                    ErrorMessage = error.Key,
                    Count = error.Count()
                });
            }

            res.IterationCompleted = iterationNumber;

            return res;
        }
    }
}
