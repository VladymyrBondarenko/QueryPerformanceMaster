using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using Microsoft.Extensions.Logging;
using System;
using Serilog.Extensions.Logging;
using Serilog;

namespace MvxStarter.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<Setup>();
        }
    }

    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override ILoggerFactory CreateLogFactory()
        {
            // serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }
    }
}
