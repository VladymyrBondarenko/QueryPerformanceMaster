using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ConnectionProvider
{
    public abstract class ConnectionProviderBase<T> : IConnectionProvider<T>
    {
        public abstract Task<T> CreateConnection(CancellationToken cancellationToken);
    }
}
