namespace QueryPerformanceMaster.Domain
{
    public class ExecuteLoadParams
    {
        public int IterationNumber { get; set; }

        public int ThreadNumber { get; set; }

        public int DelayMiliseconds { get; set; }

        public int TimeLimitMiliseconds { get; set; }

        public SqlConnectionParams ConnectionParams { get; set; }

        public string Query { get; set; }

        public IProgress<int> QueryLoadProgress { get; set; }
    }
}
