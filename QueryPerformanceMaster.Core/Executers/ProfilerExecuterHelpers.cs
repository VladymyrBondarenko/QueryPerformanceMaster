using MathNet.Numerics.Statistics;
using SqlQueryPerformanceProfiler.Executers.ExecResults;
using SqlQueryPerformanceProfiler.Profilers;
using SqlQueryPerformanceProfiler.Profilers.LoadResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueryPerformanceProfiler.Executers
{
    internal class ProfilerExecuterHelpers
    {
        public static LoadExecutedResult FillLoadExecutedResult(int iterationNumber, 
            List<LoadProfilerResult> loadProfilerResults)
        {
            var res = new LoadExecutedResult();
            
            var cpuTimes = loadProfilerResults.Select(x => x.CpuTime / 1000).ToList();
            var logicalReads = loadProfilerResults.Select(x => x.LogicalReads).ToList();
            var elapsedTimes = loadProfilerResults.Select(x => x.ElapsedTime / 1000).ToList();
            var execTimes = loadProfilerResults.Select(x => x.ExecTime).ToList();

            //calc total
            res.CpuTimeTotal = (decimal)cpuTimes.Sum();
            res.LogicalReadsTotal = (decimal)logicalReads.Sum();
            res.ElapsedTimeTotal = (decimal)elapsedTimes.Sum();

            foreach (var result in loadProfilerResults)
            {
                res.ExecTime += result.ExecTime;
            }

            // calc avg
            res.CpuTimeAvg = res.CpuTimeTotal / iterationNumber;
            res.LogicalReadsAvg = res.LogicalReadsTotal / iterationNumber;
            res.ElapsedTimeAvg = res.ElapsedTimeTotal/ iterationNumber;
            res.ExecTimeAvg = res.ExecTime / iterationNumber;

            // calc mod
            var execTimeMiliseconds = execTimes.Select(x => x.TotalMilliseconds).ToList();
            res.CpuTimeMod = (decimal)cpuTimes.Median();
            res.LogicalReadsMod = (decimal)logicalReads.Median();
            res.ElapsedTimeMod = (decimal)elapsedTimes.Median();
            res.ExecTimeMod = TimeSpan.FromMilliseconds(execTimeMiliseconds.Median());

            // calc standard dev
            res.CpuTimeStdDev = (decimal)cpuTimes.StandardDeviation();
            res.LogicalReadsStdDev = (decimal)logicalReads.StandardDeviation();
            res.ElapsedTimeStdDev = (decimal)elapsedTimes.StandardDeviation();
            res.ExecTimeStdDev = TimeSpan.FromMilliseconds(execTimeMiliseconds.StandardDeviation());

            // calc errors
            var groupedErrors = loadProfilerResults.Select(x => x.SqlQueryLoadError).GroupBy(x => x);

            foreach (var error in groupedErrors)
            {
                res.SqlQueryLoadErrors.Add(new LoadProfilerError
                {
                    ErrorMessage = error.Key,
                    Count = error.Count()
                });
            }

            // TODO: edit when add cancelation
            res.IterationCompleted = iterationNumber;

            return res;
        }
    }
}
