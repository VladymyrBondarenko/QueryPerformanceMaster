using MvvmCross;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Services;
using MvxStarter.Core.ViewModels;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters;
using QueryPerformanceMaster.App.Interfaces.LoadExecuters.Factories;
using QueryPerformanceMaster.App.Interfaces.LoadProfilers;
using QueryPerformanceMaster.App.Interfaces.SqlProviderServices;
using QueryPerformanceMaster.Core.ConnectionProvider.MsSql;
using QueryPerformanceMaster.Core.ConnectionProvider.MsSql.ConnectionSettings;
using QueryPerformanceMaster.Core.ConnectionProvider.PostgreSql;
using QueryPerformanceMaster.Core.LoadProfilers;
using QueryPerformanceMaster.Core.ProfilerExecuters;
using QueryPerformanceMaster.Core.ProfilerExecuters.ParallelProfilerExecuter;
using QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecuter;
using QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecuterWithDelay;
using QueryPerformanceMaster.Core.ProfilerExecuters.SequentialProfilerExecutorWithTimeLimit;
using QueryPerformanceMaster.Core.SqlProviderServices.Factory;

namespace MvxStarter.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxMessenger>(new MvxMessengerHub());

            // register connection services
            Mvx.IoCProvider.RegisterType<IConnectionService, ConnectionService>();
            Mvx.IoCProvider.RegisterType<IMsSqlConnectionService, MsSqlConnectionService>();
            Mvx.IoCProvider.RegisterType<IMsSqlConnectionProviderFactory, MsSqlConnectionProviderFactory>();
            Mvx.IoCProvider.RegisterType<IPostgreSqlConnectionProviderFactory, PostgreSqlConnectionProviderFactory>();

            // register sql provider services
            Mvx.IoCProvider.RegisterType<ISqlProviderManagerFactory, SqlProviderManagerFactory>();
            Mvx.IoCProvider.RegisterType<ISqlProviderService, SqlProviderService>();

            // register load profiler services
            Mvx.IoCProvider.RegisterType<ILoadProfilersFactory, LoadProfilersFactory>();
            Mvx.IoCProvider.RegisterType<IParallelProfilerExecuterFactory, ParallelProfilerExecuterFactory>();
            Mvx.IoCProvider.RegisterType<ISequentialProfilerExecuterFactory, SequentialProfilerExecuterFactory>();
            Mvx.IoCProvider.RegisterType<ISequentialProfilerExecuterWithDelayFactory, SequentialProfilerExecuterWithDelayFactory>();
            Mvx.IoCProvider.RegisterType<ISequentialProfilerExecutorWithTimeLimitFactory, SequentialProfilerExecutorWithTimeLimitFactory>();
            Mvx.IoCProvider.RegisterType<IProfilerExecuterService, ProfilerExecuterService>();

            RegisterAppStart<MainLoadViewModel>();
        }
    }
}
