using SqlQueryPerformanceProfiler.Executers;

namespace SqlQueryPerformanceProfiler.Profilers
{
    public class LoadProfilerParams
    {
        public string ConnectionString { get; set; }

        public string Query { get; set; }

        public int IterationsNumber { get; set; } = 1;

        public int ThreadsNumber { get; set; } = 1;

        public int DelayMiliseconds { get; set; }

        public SqlProvider SqlProvider { get; set; } 

        public ProfilerExecuterType ExecuterType { get; set; }
    }
}
