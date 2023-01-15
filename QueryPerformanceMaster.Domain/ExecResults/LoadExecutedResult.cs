using QueryPerformanceMaster.Domain.LoadResults;

namespace QueryPerformanceMaster.Domain.ExecResults
{
    public class LoadExecutedResult
    {
        public decimal CpuTimeTotal { get; set; }

        public List<double> CpuTimes { get; set; } = new List<double>();

        public decimal CpuTimeAvg { get; set; }

        public decimal CpuTimeMod { get; set; }

        public decimal CpuTimeStdDev { get; set; }

        public decimal LogicalReadsTotal { get; set; }

        public List<double> LogicalReads { get; set; } = new List<double>();

        public decimal LogicalReadsAvg { get; set; }

        public decimal LogicalReadsMod { get; set; }

        public decimal LogicalReadsStdDev { get; set; }

        public decimal ElapsedTimeTotal { get; set; }

        public decimal ElapsedTimeAvg { get; set; }

        public decimal ElapsedTimeMod { get; set; }

        public decimal ElapsedTimeStdDev { get; set; }

        public List<double> ElapsedTimes { get; set; } = new List<double>();

        public TimeSpan ExecTime { get; set; }

        public TimeSpan ExecTimeAvg { get; set; }

        public TimeSpan ExecTimeMod { get; set; }

        public TimeSpan ExecTimeStdDev { get; set; }

        public int IterationCompleted { get; set; }

        public List<LoadProfilerError> SqlQueryLoadErrors { get; set; } = new List<LoadProfilerError>();
    }
}
