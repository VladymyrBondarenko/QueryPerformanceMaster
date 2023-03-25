using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.Messages;
using QueryPerformanceMaster.Domain.SqlProviders;

namespace MvxStarter.Core.Models
{
    public class SqlProviderDatabaseModel
    {
        private readonly IMvxMessenger _mvxMessenger;

        public SqlProviderDatabaseModel()
        {
            CreateQueryCommand = new MvxCommand(() => CreateQuery());
            _mvxMessenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
        }

        private void CreateQuery()
        {
            _mvxMessenger.Publish(new AddedQueryEditorTabMessage(this, SqlProvider, Name, ConnectionString));
        }

        public string Name { get; set; }

        public SqlProvider SqlProvider { get; set; }

        public string ConnectionString { get; set; }

        public IMvxCommand CreateQueryCommand { get; set; }

        public bool IsExpanded { get; set; }
    }
}
