using System;

namespace PetoncleDb;

[AttributeUsage(AttributeTargets.Class)]
public class DbTable: Attribute, IDbTable
{
    /// <summary>
    /// Table Name
    /// </summary>
    public string TableName { get; }

    public string SchemaName { get; }

    public DbTable(string tableName)
    {
        TableName = tableName;
    }

    public string SqlName => TableName;
}