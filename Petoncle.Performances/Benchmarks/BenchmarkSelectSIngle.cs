using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Dapper;
using Microsoft.Data.SqlClient;
using PetoncleDb;

namespace PetonclePerformances;

[MemoryDiagnoser()]
[SimpleJob(RuntimeMoniker.Net48, baseline:true), SimpleJob(RuntimeMoniker.Net60), SimpleJob(RuntimeMoniker.Net70)]
public class Benchmark_Select_1_Row
{
    private readonly string _connectionString = "Server=localhost\\sqlexpress;Database=Petoncle;TrustServerCertificate=Yes;Trusted_Connection=True";

    
    [GlobalSetup]
    public void GlobalSetup()
    {
        if (!Petoncle.IsInitialized)
            Petoncle.Initialize<User>(DatabaseType.SqlServer, () => new SqlConnection(_connectionString));
        Petoncle.Db.Delete<User>();
        Petoncle.Db.Insert(new User {FirstName = "Bob", LastName = "Sponge", Age = 40});
    }

    [Benchmark()]
    public void Dapper()
    {
        new SqlConnection(_connectionString).Query<User>("SELECT * FROM users").AsList();
    }
    
    [Benchmark()]
    public void Pentoncle()
    {
        Petoncle.Db.Select<User>().AsList();
    }
}
//
// [MemoryDiagnoser]
// [SimpleJob(RuntimeMoniker.Net48, baseline:true), SimpleJob(RuntimeMoniker.Net60), SimpleJob(RuntimeMoniker.Net70)]
// public class BenchmarkSelect500
// {
//     [GlobalSetup]
//     public void GlobalSetup()
//     {
//         
//     }
//
//     [Benchmark()]
//     public void Dapper()
//     {
//         
//     }
//     
//     [Benchmark()]
//     public void Pentoncle()
//     {
//         
//     }
// }
//
// [MemoryDiagnoser]
// [SimpleJob(RuntimeMoniker.Net48, baseline:true), SimpleJob(RuntimeMoniker.Net60), SimpleJob(RuntimeMoniker.Net70)]
// public class BenchmarkSelect1000
// {
//     [GlobalSetup]
//     public void GlobalSetup()
//     {
//         
//     }
//
//     [Benchmark()]
//     public void Dapper()
//     {
//         
//     }
//     
//     [Benchmark()]
//     public void Pentoncle()
//     {
//         
//     }
// }