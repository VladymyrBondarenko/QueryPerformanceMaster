
namespace QueryPerformanceMaster.Domain.ConnectionSettings
{
    public class PostreSqlConnectionSettings
    {
        public string Database { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Server { get; set; }

        public int Port { get; set; }

        public int ConnectTimeout { get; set; }

        public bool EnablePooling { get; set; }

        public int MaxPoolSize { get; set; }

        public PostreSqlConnectionSettings()
        {
            Server = "(local)";
            EnablePooling = true;
        }
    }
}
