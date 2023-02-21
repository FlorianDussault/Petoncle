namespace PetoncleUT.SqlServer.Objects;


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
    public bool? Enabled { get; set; }
    
    [DbColumn("create_date")]
    public DateTime? CreateDate { get; set; }
}

public class UserAnonymous
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public int Age { get; set; }
    
    public bool Enabled { get; set; }
}