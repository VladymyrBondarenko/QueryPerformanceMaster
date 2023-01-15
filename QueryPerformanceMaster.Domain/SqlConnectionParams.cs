namespace QueryPerformanceMaster.Domain
{
    public class SqlConnectionParams
    {
        public string ConnectionString { get; set; }

        public SqlProvider SqlProvider { get; set; }
    }
}
