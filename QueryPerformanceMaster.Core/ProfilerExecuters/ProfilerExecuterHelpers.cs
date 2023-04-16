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
            
            res.CpuTimes = loadProfilerResults.Select(x => x.CpuTime / 1000d).ToList();
            res.LogicalReads = loadProfilerResults.Select(x => x.LogicalReads).ToList();
            res.ElapsedTimes = loadProfilerResults.Select(x => x.ElapsedTime / 1000d).ToList();

            //calc total
            res.CpuTimeTotal = Math.Round((decimal)res.CpuTimes.Sum(), 4);
            res.LogicalReadsTotal = Math.Round((decimal)res.LogicalReads.Sum(), 4);
            res.ElapsedTimeTotal = Math.Round((decimal)res.ElapsedTimes.Sum(), 4);

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
            var execTimeMiliseconds = loadProfilerResults.Select(x => x.ExecTime.TotalMilliseconds).ToList();
            res.CpuTimeMod = res.CpuTimes.Count > 0 ? Math.Round((decimal)res.CpuTimes.Median(), 4) : 0;
            res.LogicalReadsMod = res.LogicalReads.Count > 0 ? Math.Round((decimal)res.LogicalReads.Median(), 4) : 0;
            res.ElapsedTimeMod = res.ElapsedTimes.Count > 0 ? Math.Round((decimal)res.ElapsedTimes.Median(), 4) : 0;
            res.ExecTimeMod = execTimeMiliseconds.Count > 0 ? TimeSpan.FromMilliseconds(execTimeMiliseconds.Median()) : TimeSpan.Zero;

            // calc standard dev
            res.CpuTimeStdDev = res.CpuTimes.Count > 1 ? Math.Round((decimal)res.CpuTimes.StandardDeviation(), 4) : 0;
            res.LogicalReadsStdDev = res.LogicalReads.Count > 1 ? Math.Round((decimal)res.LogicalReads.StandardDeviation(), 4) : 0;
            res.ElapsedTimeStdDev = res.ElapsedTimes.Count > 1 ? Math.Round((decimal)res.ElapsedTimes.StandardDeviation(), 4) : 0;
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
