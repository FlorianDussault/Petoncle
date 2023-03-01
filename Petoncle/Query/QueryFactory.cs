namespace PetoncleDb;

internal static class QueryFactory
{
    public static SelectBase Select(Connection connection, PObject pObject)
    {
        if (connection.DatabaseType == DatabaseType.SqlServer)
            return new SelectSqlServer(pObject);
        return null;
    }
    
    public static InsertBase Insert(Connection connection, PObject pObject, object obj)
    {
        if (connection.DatabaseType == DatabaseType.SqlServer)
            return new InsertSqlServer(pObject, obj);
        return null;
    }
    
    public static UpdateBase Update(Connection connection, PObject pObject, object obj)
    {
        if (connection.DatabaseType == DatabaseType.SqlServer)
            return new UpdateSqlServer(pObject, obj);
        return null;
    }
    
    public static DeleteBase Delete(Connection connection, PObject pObject, object obj)
    {
        if (connection.DatabaseType == DatabaseType.SqlServer)
            return new DeleteSqlServer(pObject, obj);
        return null;
    }

    public static TruncateBase Truncate(Connection connection, PObject pObject)
    {
        if (connection.DatabaseType == DatabaseType.SqlServer)
            return new TruncateSqlServer(pObject);
        return null;
    }
}