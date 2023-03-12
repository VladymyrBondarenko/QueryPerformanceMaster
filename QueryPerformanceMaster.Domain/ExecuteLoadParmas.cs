namespace QueryPerformanceMaster.Domain
{
    public class ExecuteLoadParmas
    {
        public int IterationNumber { get; set; }

        public int ThreadNumber { get; set; }

        public int DelayMiliseconds { get; set; }

        public int TimeLimitMiliseconds { get; set; }

        public SqlConnectionParams ConnectionParams { get; set; }

        public string Query { get; set; }
    }
}
