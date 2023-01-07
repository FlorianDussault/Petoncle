using System;

namespace PetoncleDb;

[AttributeUsage(AttributeTargets.Class)]
public class DbTableSqlServer: Attribute, IDbTable
{

    public string TableName { get; }
    public string SchemaName { get; }

    /// <summary>
    /// Table Name
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="schemaName"></param>
    /// <param name="tableName"></param>
    public DbTableSqlServer(string schemaName, string tableName)
    {
        SchemaName = schemaName;
        TableName = tableName;
    }

    public DbTableSqlServer(string tableName)
    {
        SchemaName = null;
        TableName = tableName;
    }
}