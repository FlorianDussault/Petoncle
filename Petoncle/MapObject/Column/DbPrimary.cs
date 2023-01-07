using System;

namespace PetoncleDb;

[AttributeUsage(AttributeTargets.Property)]
public class DbPrimary : DbColumn
{
    public DbPrimary()
    {
    }
    
    public DbPrimary(string columnName) : base(columnName)
    {
    }
}