using SqlQueryPerformanceProfiler.Profilers.LoadResults;

namespace SqlQueryPerformanceProfiler.Executers.ExecResults
{
    public class LoadExecutedResult
    {
        public double CpuTimeTotal { get; set; }

        public List<double> CpuTimes { get; set; } = new List<double>();

        public double CpuTimeAvg { get; set; }

        public double CpuTimeMod { get; set; }

        public double CpuTimeStdDev { get; set; }

        public double LogicalReadsTotal { get; set; }

        public List<double> LogicalReads { get; set; } = new List<double>();

        public double LogicalReadsAvg { get; set; }

        public double LogicalReadsMod { get; set; }

        public double LogicalReadsStdDev { get; set; }

        public double ElapsedTimeTotal { get; set; }

        public double ElapsedTimeAvg { get; set; }

        public double ElapsedTimeMod { get; set; }

        public double ElapsedTimeStdDev { get; set; }

        public List<double> ElapsedTimes { get; set; } = new List<double>();

        public TimeSpan ExecTime { get; set; }

        public TimeSpan ExecTimeAvg { get; set; }

        public TimeSpan ExecTimeMod { get; set; }

        public TimeSpan ExecTimeStdDev { get; set; }

        public int IterationCompleted { get; set; }

        public List<LoadProfilerError> SqlQueryLoadErrors { get; set; } = new List<LoadProfilerError>();
    }
}
