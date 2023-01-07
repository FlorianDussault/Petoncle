using System;

namespace PetoncleDb;

[AttributeUsage(AttributeTargets.Property)]
public class DbColumn : Attribute
{
    internal string ColumnName { get; }

    public DbColumn()
    {
        
    }
    public DbColumn(string columnName)
    {
        ColumnName = columnName;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class DbAutoIncrement : DbReadOnly
{
}

[AttributeUsage(AttributeTargets.Property)]
public class DbReadOnly : Attribute
{
}