using QueryPerformanceMaster.Domain;

namespace QueryPerformanceMaster.App.Interfaces.LoadProfilers
{
    public interface ILoadProfilersFactory
    {
        ILoadProfiler GetLoadProfiler(SqlConnectionParams connectionParams);
    }
}