using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Domain.SqlProviders
{
    public class DropBuffersAndCacheResult
    {
        public DropBuffersAndCacheResult(bool success = true)
        {
            Success = success;
        }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
