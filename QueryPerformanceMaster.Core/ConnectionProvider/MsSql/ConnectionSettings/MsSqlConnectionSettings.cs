using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace QueryPerformanceMaster.Core.ConnectionProvider.MsSql.ConnectionSettings
{
    [Serializable]
    [DataContract]
    public class MsSqlConnectionSettings
    {
        [DataMember]
        public string Database { get; set; }

        [DataMember]
        public bool IntegratedAuth { get; set; }

        [DataMember]
        public bool AzureMFA { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Server { get; set; }

        [DataMember]
        public ApplicationIntent ApplicationIntent { get; set; }

        [DataMember]
        public int ConnectTimeout { get; set; }

        [DataMember]
        public bool EnablePooling { get; set; }

        [DataMember]
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
            Login = string.Empty;
            Password = string.Empty;
            Database = string.Empty;
            ConnectTimeout = 0;
            MaxPoolSize = 0;
            EnablePooling = true;
        }
    }
}
