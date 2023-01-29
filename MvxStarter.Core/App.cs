using MvvmCross;
using MvvmCross.ViewModels;
using MvxStarter.Core.ViewModels;
using QueryPerformanceMaster.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<ISqlProviderService, SqlProviderService>();

            RegisterAppStart<MainLoadViewModel>();
        }
    }
}
