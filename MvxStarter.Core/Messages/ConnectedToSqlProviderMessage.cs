using MvvmCross.Plugin.Messenger;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Messages
{
    public class ConnectedToSqlProviderMessage : MvxMessage
    {
        public ConnectedToSqlProviderMessage(object sender, SqlProvider sqlProvider, List<string> databases, string connectionString)
            : base(sender)
        {
            Databases = databases;
            SqlProvider = sqlProvider;
            ConnectionString = connectionString;
        }

        public List<string> Databases { get; private set; }

        public SqlProvider SqlProvider { get; private set; }

        public string ConnectionString { get; set; }
    }
}
