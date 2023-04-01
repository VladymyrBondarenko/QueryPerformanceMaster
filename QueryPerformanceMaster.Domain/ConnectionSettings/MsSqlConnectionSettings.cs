using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace QueryPerformanceMaster.Domain.ConnectionSettings
{
    public class MsSqlConnectionSettings
    {
        public string Database { get; set; }

        public bool IntegratedAuth { get; set; }

        public bool AzureMFA { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Server { get; set; }

        public ApplicationIntent ApplicationIntent { get; set; }

        public int ConnectTimeout { get; set; }

        public bool EnablePooling { get; set; }

        public int MaxPoolSize { get; set; }

        public bool RequiresPassword
        {
            get
            {
                return !IntegratedAuth && !AzureMFA;
            }
        }

        public MsSqlConnectionSettings()
        {
            Server = "(local)";
            IntegratedAuth = true;
            ApplicationIntent = ApplicationIntent.ReadWrite;
            EnablePooling = true;
        }
    }
}
