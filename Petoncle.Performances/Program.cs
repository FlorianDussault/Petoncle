// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using Dapper;
using LazySql;
using Microsoft.Data.SqlClient;
using PetoncleDb;
using PetonclePerformances;


//new BenchmarkTester().Other();
//BenchmarkRunner.Run<Benchmark_Select_1_Row>();
//return;
BenchmarkRunner.Run(
    typeof(User).Assembly, // all benchmarks from given assembly are going to be executed
    ManualConfig
        .Create(DefaultConfig.Instance)
        .With(ConfigOptions.JoinSummary)
        .With(ConfigOptions.DisableLogFile).WithOrderer(new DefaultOrderer(SummaryOrderPolicy.Method, MethodOrderPolicy.Alphabetical)));

// [MemoryDiagnoser]
public class BenchmarkTester
{
    private readonly string _connectionString = "Server=localhost\\sqlexpress;Database=Petoncle;TrustServerCertificate=Yes;Trusted_Connection=True";

    public void Other()
    {
        Executor<User>(u => u.Id == 1);
        Executor<User>(u => u.Id == 1);
        Executor<User>(u => u.Id == 2);
    }

    private void Executor<T>(Expression<Func<T, object>> expression)
    {
        var a = expression.ToString();
        Console.WriteLine(a);
    }
    
    public BenchmarkTester()
    {
        PetoncleDb.Petoncle.Initialize<User>(DatabaseType.SqlServer,() => new SqlConnection(_connectionString));
        LazyClient.Initialize(_connectionString, typeof(UserLazy));
    }
    // [Benchmark]
    public void RunDapper()
    {
        PetoncleDb.Petoncle.Db.Delete<User>();
        
        SqlConnection sqlConnection = new(_connectionString);
            var identity = sqlConnection.QuerySingle<int>("INSERT INTO [dbo].[users] (FirstName, LastName, Age) output inserted.Id VALUES (@FirstName, @LastName, @Age);"
                , new User { FirstName = "Florian", LastName = "Dussault", Age = 10});
    }
    // [Benchmark]
    public void RunPetoncle()
    {
        PetoncleDb.Petoncle.Db.Delete<User>();
        PetoncleDb.Petoncle.Db.Insert(new User {FirstName = "Florian", LastName = "Dussault", Age = 10});

    }
    
   // [Benchmark]
    public void RunLazySql()
    {
        PetoncleDb.Petoncle.Db.Delete<User>();
        LazyClient.Insert(new UserLazy {FirstName = "Florian", LastName = "Dussault", Age = 10});

    }
   
}