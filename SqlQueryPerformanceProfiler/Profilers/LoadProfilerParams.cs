namespace SqlQueryPerformanceProfiler.Profilers
{
    public class LoadProfilerParams
    {
        public string ConnectionString { get; set; }

        public string Query { get; set; }

        public int IterationsNumber { get; set; }

        public int ThreadsNumber { get; set; }

        public int DelayMiliseconds { get; set; }
    }
}
