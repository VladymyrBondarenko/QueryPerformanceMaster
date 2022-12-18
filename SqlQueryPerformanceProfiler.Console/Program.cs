// See https://aka.ms/new-console-template for more information
using SqlQueryPerformanceProfiler.Profilers;





var loadParams = new SqlQueryLoadParams
{
    ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PersonnelManagementDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
    IterationsNumber = 200,
    ThreadsNumber = 2,
    Query = "select DepartmentTitle from Departments"
};

var queryLoadProfiler = new MssqlLoadProfiler(loadParams);
var loadResult = queryLoadProfiler.ExecuteLoad();

Console.WriteLine(
    $@"
        CpuTimeAvg: {loadResult.CpuTimeAvg};
        CpuTimeMod: {loadResult.CpuTimeMod}
        ElapsedTime: {loadResult.ElapsedTimeTotal};
        ElapsedTimeMod: {loadResult.ElapsedTimeMod};
        ExecTime: {loadResult.ExecTime};
        LogicalReadsAvg: {loadResult.LogicalReadsAvg};
        LogicalReadsMod: {loadResult.LogicalReadsMod};
        Errors: {string.Join(",", loadResult.SqlQueryLoadErrors.Select(x => x.ErrorMessage))}
    ");