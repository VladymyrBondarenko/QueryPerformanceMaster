using MvvmCross;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Services;
using MvxStarter.Core.ViewModels;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Core.ConnectionProvider.MsSql;
using QueryPerformanceMaster.Core.ConnectionProvider.MsSql.ConnectionSettings;
using QueryPerformanceMaster.Core.SqlProviderServices.Factory;

namespace MvxStarter.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxMessenger>(new MvxMessengerHub());
            Mvx.IoCProvider.RegisterType<IMsSqlConnectionProviderFactory, MsSqlConnectionProviderFactory>();
            Mvx.IoCProvider.RegisterType<ISqlProviderManagerFactory, SqlProviderManagerFactory>();
            Mvx.IoCProvider.RegisterType<ISqlProviderService, SqlProviderService>();
            Mvx.IoCProvider.RegisterType<IMsSqlConnectionService, MsSqlConnectionService>();

            RegisterAppStart<MainLoadViewModel>();
        }
    }
}
