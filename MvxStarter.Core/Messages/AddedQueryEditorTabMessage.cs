using MvvmCross.Plugin.Messenger;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.Messages
{
    public class AddedQueryEditorTabMessage : MvxMessage
    {
        public AddedQueryEditorTabMessage(object sender, SqlProvider sqlProvider, string database,
            string connectionString)
           : base(sender)
        {
            SqlProvider = sqlProvider;
            Database = database;
            ConnectionString = connectionString;
        }

        public SqlProvider SqlProvider { get; private set; }

        public string Database { get; private set; }

        public string ConnectionString { get; set; }
    }
}
