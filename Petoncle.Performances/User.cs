using LazySql;
using PetoncleDb;

namespace PetonclePerformances;

[DbTableSqlServer("dbo", "users")]
public class User : DbObject
{
    [DbPrimary, DbAutoIncrement]
    public int Id { get; set; }
    
    [DbColumn]
    public string FirstName { get; set; }
    
    [DbColumn]
    public string LastName { get; set; }
    
    [DbColumn]
    public int Age { get; set; }
    
    [DbColumn, DbReadOnly]
    public bool Enabled { get; set; }

    [DbColumn("create_date")]
    public DateTime CreateDate { get; set; }
}
[LazyTable("users")]
public class UserLazy : LazyBase
{
    [LazyColumn(SqlType.Int), PrimaryKey(true)]
    public int Id { get; set; }
    
    [LazyColumn(SqlType.NVarChar)]
    public string FirstName { get; set; }
    
    [LazyColumn(SqlType.NVarChar)]
    public string LastName { get; set; }
    
    [LazyColumn(SqlType.Int)]
    public int Age { get; set; }
    
    [LazyColumn(SqlType.Bit)]
    public bool Enabled { get; set; }
}