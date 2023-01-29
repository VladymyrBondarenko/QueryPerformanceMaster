using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Domain.SqlProviders
{
    public class SqlProviderModel
    {
        public SqlProvider SqlProvider { get; set; }

        public string SqlProviderTitle { get; set; }

        public string SqlProviderIcon { get; set; }
    }
}
