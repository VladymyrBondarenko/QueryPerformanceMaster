// See https://aka.ms/new-console-template for more information
using SqlQueryPerformanceProfiler.Executers;
using SqlQueryPerformanceProfiler.Profilers;





var loadParams = new LoadProfilerParams
{
    ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PersonnelManagementDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
    IterationsNumber = 5,
    ThreadsNumber = 2,
    Query = "select DepartmentTitle from Departments"
};

var mssqlLoadProfiler = new MssqlLoadProfiler(loadParams);

//var seqProfilerExecuter = new SequentialProfilerExecuter(loadParams, mssqlLoadProfiler);
//var loadResult = seqProfilerExecuter.ExecuteLoad();

//loadParams.DelayMiliseconds = 10;
//var seqProfilerExecuter = new SequentialProfilerExecuterWithDelay(loadParams, mssqlLoadProfiler);
//var loadResult = seqProfilerExecuter.ExecuteLoad();

loadParams.ThreadsNumber = 2;
var parallelProfilerExecuter = new ParallelProfilerExecuter(loadParams, mssqlLoadProfiler);
var loadResult = await parallelProfilerExecuter.ExecuteLoadAsync(CancellationToken.None);

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