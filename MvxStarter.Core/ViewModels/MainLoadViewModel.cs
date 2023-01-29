using MvvmCross.ViewModels;
using MvxStarter.Core.Models;
using QueryPerformanceMaster.Core;
using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.ViewModels
{
    public class MainLoadViewModel : MvxViewModel
    {
        public MainLoadViewModel(ISqlProviderService sqlProviderService)
        {
            _sqlProviderService = sqlProviderService;
        }

        private readonly ISqlProviderService _sqlProviderService;

        private SqlProvidersModel _sqlProvidersModel;

        public SqlProvidersModel SqlProvidersModel
        {
            get { return _sqlProvidersModel; }
            set { SetProperty(ref _sqlProvidersModel, value); }
        }

        public override Task Initialize()
        {
            var sqlProviders = _sqlProviderService.GetSqlProviders();
            SqlProvidersModel = new SqlProvidersModel
            {
                SqlProviderModels = new List<object>(
                    sqlProviders.Select(x => new Models.SqlProviderModel { Name = x.SqlProviderTitle }))
            };

            return base.Initialize();
        }
    }
}
