// See https://aka.ms/new-console-template for more information
using SqlQueryPerformanceProfiler.Executers;
using SqlQueryPerformanceProfiler.Executers.ProfilerExecuters;
using SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.ParallelProfilerExecuter;
using SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.SequentialProfilerExecuter;
using SqlQueryPerformanceProfiler.Executers.ProfilerExecuters.SequentialProfilerExecutorWithTimeLimit;
using SqlQueryPerformanceProfiler.Profilers;
using SqlQueryPerformanceProfiler.Profilers.LoadProfilers;

async Task testMsSql()
{
    //var query =
    //    @"
    //    select * from sales.orders 
    //    where customer_id = 
    //        (select max(customer_id) from sales.customers t where t.first_name = 'Tameka' and t.last_name = 'Fisher')";

    var query =
        @"
    select sales.orders.order_id 
    from sales.customers
    join sales.orders on sales.orders.customer_id = sales.customers.customer_id";


    var connectionParams = new SqlConnectionParams
    {
        ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BikeStores",
        SqlProvider = SqlProvider.SqlServer
    };

    var loadProfilersFactory = new LoadProfilersFactory();
    var mssqlLoadProfiler = loadProfilersFactory.GetLoadProfiler(connectionParams);

    //var seqProfilerExecuter = new SequentialProfilerExecuter(mssqlLoadProfiler);
    //var loadResult = await seqProfilerExecuter.ExecuteLoadAsync(query, 100);

    var seqProfilerExecuter = new SequentialProfilerExecutorWithTimeLimit(mssqlLoadProfiler);
    var loadResult = await seqProfilerExecuter.ExecuteLoadAsync(query, 200, 100);

    ////var seqProfilerExecuter = new SequentialProfilerExecuter(loadParams, mssqlLoadProfiler);
    ////var loadResult = seqProfilerExecuter.ExecuteLoad();

    ////loadParams.DelayMiliseconds = 10;
    ////var seqProfilerExecuter = new SequentialProfilerExecuterWithDelay(loadParams, mssqlLoadProfiler);
    ////var loadResult = await seqProfilerExecuter.ExecuteLoadAsync(CancellationToken.None);

    //loadParams.SqlProvider = SqlProvider.SqlServer;
    //loadParams.ExecuterType = ProfilerExecuterType.SequentialExecutor;

    //var loadProfilersFactory = new LoadProfilersFactory();
    //var executorsFactory = new LoadProfilerExecutorsFactory(loadProfilersFactory);

    //var parallelProfilerExecuter = executorsFactory.GetProfilerExecuter(loadParams);
    //var loadResult =  await parallelProfilerExecuter.ExecuteLoadAsync(CancellationToken.None);

    ////var parallelProfilerExecuter = new ParallelProfilerExecuter(loadParams, mssqlLoadProfiler);
    ////var loadResult = await parallelProfilerExecuter.ExecuteLoadAsync(CancellationToken.None);

    Console.WriteLine(
        $@"
        CpuTimeTotal: {loadResult.CpuTimeTotal};
        CpuTimeAvg: {loadResult.CpuTimeAvg};
        CpuTimeMod: {loadResult.CpuTimeMod};
        CpuTimeStdDev: {loadResult.CpuTimeStdDev};
        ElapsedTime: {loadResult.ElapsedTimeTotal};
        ElapsedTimeAvg: {loadResult.ElapsedTimeAvg};
        ElapsedTimeMod: {loadResult.ElapsedTimeMod};
        ExecTime: {loadResult.ExecTime};
        ExecTimeAvg: {loadResult.ExecTimeAvg};
        ExecTimeMod: {loadResult.ExecTimeMod};
        ExecTimeStdDev: {loadResult.ExecTimeStdDev};
        LogicalReadsTotal: {loadResult.LogicalReadsTotal};
        LogicalReadsAvg: {loadResult.LogicalReadsAvg};
        LogicalReadsMod: {loadResult.LogicalReadsMod};
        LogicalReadsStdDev: {loadResult.LogicalReadsStdDev};
        Errors: {string.Join(",", loadResult.SqlQueryLoadErrors.Select(x => x.ErrorMessage))}
    ");
}

async Task testPostgreSql()
{
    //var query =
    //    @"select id from customer_order where product_id = (select id from product where product_name = 'product 54')";

    var query =
        @"select id from product where product_name = 'product 54'";

    var connectionParams = new SqlConnectionParams
    {
        ConnectionString = "Server=127.0.0.1;Port=5432;Database=StoreDb;User Id=postgres;Password=1777897;",
        SqlProvider = SqlProvider.PostgreSql
    };

    var loadProfilersFactory = new LoadProfilersFactory();
    var postgreSqlLoadProfiler = loadProfilersFactory.GetLoadProfiler(connectionParams);

    var seqProfilerExecuter = new SequentialProfilerExecuter(postgreSqlLoadProfiler);
    var loadResult = await seqProfilerExecuter.ExecuteLoadAsync(query, 100);

    Console.WriteLine(
        $@"
        CpuTimeTotal: {loadResult.CpuTimeTotal};
        CpuTimeAvg: {loadResult.CpuTimeAvg};
        CpuTimeMod: {loadResult.CpuTimeMod};
        CpuTimeStdDev: {loadResult.CpuTimeStdDev};
        ElapsedTime: {loadResult.ElapsedTimeTotal};
        ElapsedTimeAvg: {loadResult.ElapsedTimeAvg};
        ElapsedTimeMod: {loadResult.ElapsedTimeMod};
        ElapsedTimeStdDev: {loadResult.ElapsedTimeStdDev};
        ExecTime: {loadResult.ExecTime};
        ExecTimeAvg: {loadResult.ExecTimeAvg};
        ExecTimeMod: {loadResult.ExecTimeMod};
        ExecTimeStdDev: {loadResult.ExecTimeStdDev};
        LogicalReadsTotal: {loadResult.LogicalReadsTotal};
        LogicalReadsAvg: {loadResult.LogicalReadsAvg};
        LogicalReadsMod: {loadResult.LogicalReadsMod};
        LogicalReadsStdDev: {loadResult.LogicalReadsStdDev};
        Errors: {string.Join(",", loadResult.SqlQueryLoadErrors.Select(x => x.ErrorMessage))}
    ");
}

await testPostgreSql();
//await testMsSql();