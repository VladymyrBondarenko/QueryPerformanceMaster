using Microsoft.Extensions.Configuration;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvxStarter.Core.Services;
using MvxStarter.Core.ViewModels;

namespace MvxStarter.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            //var configuration = addConfiguration();

            //var sqlProviderOptions = new SqlProviderOptions();
            //configuration.GetSection(nameof(SqlProviderOptions)).Bind()

            Mvx.IoCProvider.RegisterType<ISqlProviderService, SqlProviderService>();

            RegisterAppStart<MainLoadViewModel>();
        }

        //private IConfiguration addConfiguration()
        //{
        //    IConfigurationBuilder builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json");

        //    return builder.Build();
        //}
    }
}
