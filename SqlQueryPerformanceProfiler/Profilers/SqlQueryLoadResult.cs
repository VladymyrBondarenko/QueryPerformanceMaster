namespace SqlQueryPerformanceProfiler.Profilers
{
    public class SqlQueryLoadResult
    {
        public double CpuTimeTotal { get; set; }

        public List<double> CpuTimes { get; set; } = new List<double>();

        public double CpuTimeAvg { get; set; }

        public double CpuTimeMod { get; set; }

        public double LogicalReadsTotal { get; set; }

        public List<double> LogicalReads { get; set; } = new List<double>();

        public double LogicalReadsAvg { get; set; }

        public double LogicalReadsMod { get; set; }

        public double ElapsedTimeTotal { get; set; }

        public double ElapsedTimeMod { get; set; }

        public List<double> ElapsedTimes { get; set; } = new List<double>();

        public TimeSpan ExecTime { get; set; }

        public int IterationCompleted { get; set; }

        public List<SqlQueryLoadError> SqlQueryLoadErrors { get; set; } = new List<SqlQueryLoadError>();
    }
}
