namespace SqlQueryPerformanceProfiler.Profilers.LoadResults
{
    public class LoadProfilerResult
    {
        public double CpuTime { get; set; }

        public double LogicalReads { get; set; }

        public double ElapsedTime { get; set; }

        public TimeSpan ExecTime { get; set; }

        public string SqlQueryLoadError { get; set; }
    }
}
