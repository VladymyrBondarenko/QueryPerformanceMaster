using MathNet.Numerics.Statistics;
using QueryPerformanceMaster.Domain.ExecResults;
using QueryPerformanceMaster.Domain.LoadResults;
using System.Security.Cryptography.X509Certificates;

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
            res.CpuTimeTotal = (decimal)cpuTimes.Sum() / 1000m;
            res.LogicalReadsTotal = (decimal)logicalReads.Sum();
            res.ElapsedTimeTotal = (decimal)elapsedTimes.Sum() / 1000m;

            foreach (var result in loadProfilerResults)
            {
                res.ExecTime += result.ExecTime;
            }

            // calc avg
            res.CpuTimeAvg = res.CpuTimeTotal / iterationNumber;
            res.LogicalReadsAvg = res.LogicalReadsTotal / iterationNumber;
            res.ElapsedTimeAvg = res.ElapsedTimeTotal / iterationNumber;
            res.ExecTimeAvg = res.ExecTime / iterationNumber;

            // calc mod
            var execTimeMiliseconds = execTimes.Select(x => x.TotalMilliseconds).ToList();
            res.CpuTimeMod = (decimal)cpuTimes.Median();
            res.LogicalReadsMod = (decimal)logicalReads.Median();
            res.ElapsedTimeMod = (decimal)elapsedTimes.Median();
            res.ExecTimeMod = TimeSpan.FromMilliseconds(execTimeMiliseconds.Median());

            // calc standard dev
            res.CpuTimeStdDev = cpuTimes.Count > 1 ? (decimal)cpuTimes.StandardDeviation() : 0;
            res.LogicalReadsStdDev = logicalReads.Count > 1 ? (decimal)logicalReads.StandardDeviation() : 0;
            res.ElapsedTimeStdDev = elapsedTimes.Count > 1 ? (decimal)elapsedTimes.StandardDeviation() : 0;
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
