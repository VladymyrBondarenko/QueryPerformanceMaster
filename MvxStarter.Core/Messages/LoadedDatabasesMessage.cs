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
    public class LoadedDatabasesMessage : MvxMessage
    {
        public LoadedDatabasesMessage(object sender, SqlProvider sqlProvider, List<string> databases)
            : base(sender)
        {
            Databases = databases;
            SqlProvider = sqlProvider;
        }

        public List<string> Databases { get; private set; }

        public SqlProvider SqlProvider { get; private set; }
    }
}
