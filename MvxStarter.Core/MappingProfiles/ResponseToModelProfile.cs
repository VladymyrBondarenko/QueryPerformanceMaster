using AutoMapper;
using MvxStarter.Core.ViewModels;
using QueryPerformanceMaster.Domain.ExecResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.MappingProfiles
{
    internal class ResponseToModelProfile : Profile
    {
        public ResponseToModelProfile()
        {
            CreateMap<LoadExecutedResult, LoadResultsViewModel>();
        }
    }
}
