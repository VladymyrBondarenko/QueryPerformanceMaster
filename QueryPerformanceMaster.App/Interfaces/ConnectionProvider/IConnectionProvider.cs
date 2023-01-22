namespace QueryPerformanceMaster.App.Interfaces.ConnectionProvider
{
    public interface IConnectionProvider<T>
    {
        Task<T> CreateConnection(CancellationToken cancellationToken = default);
    }
}