using MvvmCross.Navigation;
using MvvmCross;
using QueryPerformanceMaster.Domain.SqlProviders;
using MvxStarter.Core.ViewModels.ConnectionParamsViewModels;
using MvvmCross.Commands;
using MvxStarter.Core.Services;

namespace MvxStarter.Core.Models
{
    public class SqlProviderModel
    {
        public SqlProviderModel()
        {
            var providerService = Mvx.IoCProvider.Resolve<ISqlProviderManager>();
            OpenConnectionParamsViewCommand = new MvxCommand(async () => await providerService.OpenConnectionParamsView(SqlProvider, ConnectionString));
        }

        public IMvxCommand OpenConnectionParamsViewCommand { get; set; }

        public string Name { get; set; }

        public string IconPath { get; set; }

        public SqlProvider SqlProvider { get; set; }

        public string ConnectionString { get; set; }

        private List<SqlProviderDatabaseModel> _databases;

        public List<SqlProviderDatabaseModel> Databases
        {
            get { return _databases; }
            set
            {
                _databases = value;
            }
        }

        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                if (value && !Databases.Any(x => !string.IsNullOrWhiteSpace(x.Name)))
                {
                    OpenConnectionParamsViewCommand.Execute();
                }
                _IsExpanded = value;
            }
        }
    }
}
