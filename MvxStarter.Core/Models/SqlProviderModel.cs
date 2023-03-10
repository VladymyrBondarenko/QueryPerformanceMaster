using MvvmCross.Navigation;
using MvvmCross;
using QueryPerformanceMaster.Domain.SqlProviders;
using MvxStarter.Core.ViewModels.ConnectionParamsViewModels;

namespace MvxStarter.Core.Models
{
    public class SqlProviderModel
    {
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
                    var navManager = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
                    navManager.Navigate<MsSqlConnectionParamsViewModel>();
                }
                _IsExpanded = value;
            }
        }
    }
}
