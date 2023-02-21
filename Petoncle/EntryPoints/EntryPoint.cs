using System;
using System.Linq.Expressions;

namespace PetoncleDb;

public abstract class EntryPoint
{
    private Petoncle Petoncle { get; }
    private Connection Connection { get; }
    internal EntryPoint(Petoncle petoncle, Connection connection)
    {
        Petoncle = petoncle;
        Connection = connection;
    }
    
    #region Select
    public IPetoncleEnumerable<T> Select<T>() => Select<T>(null, null, null);
    public IPetoncleEnumerable<T> Select<T>(string tableName) => Select<T>(null, tableName, null);
    public IPetoncleEnumerable<dynamic> Select(string tableName) => Select<dynamic>(null, tableName, null);


    public IPetoncleEnumerable<T> Select<T>(Expression<Func<T, bool>> where) => Select<T>(null, null, where);
    public IPetoncleEnumerable<T> Select<T>(string tableName, Expression<Func<T, bool>> where) => Select<T>(null, tableName, where);
    
    
    
    private IPetoncleEnumerable<T> Select<T>(string schemaName, string tableName, Expression whereExpression)
    {
        // TODO: Check performances
        Type type = typeof(T);
        if (tableName == null && type == typeof(object))
            throw new PetoncleException($"You cannot call the {nameof(Select)} method with a dynamic type without a table name in argument");
        
        Petoncle.ObjectManager.PrepareDbObject(typeof(T), schemaName, tableName, out PObject pObject);

        if (pObject == null)
            throw new PetoncleException($"Unknown object type: {typeof(T).FullName}");
        
        return new PetoncleEnumerable<T>(Connection, pObject, whereExpression);
    }
    #endregion

    #region Insert
    public int Insert<T>(T obj) => Insert<T>(null, null, obj);
    
    private int Insert<T>(string schemaName, string tableName, T obj)
    {
        // TODO: Check performances
        Type type = typeof(T);
        if (tableName == null && type == typeof(object))
            throw new PetoncleException($"You cannot call the {nameof(Insert)} method with a dynamic type without a table name in argument");
        
        Petoncle.ObjectManager.PrepareDbObject(typeof(T), schemaName, tableName, out PObject pObject);
        if (pObject == null)
            throw new PetoncleException($"Unknown object type: {typeof(T).FullName}");
        
        InsertBase insertBase = QueryFactory.Insert(Connection, pObject, obj);
        QueryBuilder queryBuilder = new(pObject, Connection.DatabaseType);
        insertBase.Build(ref queryBuilder);
        
        using SqlClient sqlClient = new(Connection);
        return sqlClient.ExecuteNonQuery(queryBuilder);
    }
    #endregion

    #region Delete

    public int Delete<T>() => Delete<T>(null, null, null);

    private int Delete<T>(string schemaName, string tableName, object obj)
    {
        Type type = typeof(T);
        if (tableName == null && type == typeof(object))
            throw new PetoncleException($"You cannot call the {nameof(Delete)} method with a dynamic type without a table name in argument");
        
        Petoncle.ObjectManager.PrepareDbObject(typeof(T), schemaName, tableName, out PObject pObject);
        if (pObject == null)
            throw new PetoncleException($"Unknown object type: {typeof(T).FullName}");
        
        DeleteBase insertBase = QueryFactory.Delete(Connection, pObject, obj);
        QueryBuilder queryBuilder = new(pObject, Connection.DatabaseType);
        insertBase.Build(ref queryBuilder);
        
        using SqlClient sqlClient = new(Connection);
        return sqlClient.ExecuteNonQuery(queryBuilder);
    }
    
    #endregion

    #region Truncate

    public void Truncate<T>() => Truncate(typeof(T), null, null);

    public void Truncate(string tableName) => Truncate(null, null, tableName);
    
    public void Truncate(string schemaName, string tableName) => Truncate(null, schemaName, tableName);

    private void Truncate(Type type, string schemaName, string tableName)
    {
        if (tableName == null && type == typeof(object))
            throw new PetoncleException($"You cannot call the {nameof(Truncate)} method with a dynamic type without a table name in argument");
        
        if (type == null && tableName == null)
            throw new PetoncleException($"Provide a table name");
        
        Petoncle.ObjectManager.PrepareDbObject(type ?? typeof(object), schemaName, tableName, out PObject pObject);
        TruncateBase truncateBase = QueryFactory.Truncate(Connection, pObject);
        QueryBuilder queryBuilder = new(pObject, Connection.DatabaseType);
        truncateBase.Build(ref queryBuilder);
        
        using SqlClient sqlClient = new(Connection);
        sqlClient.ExecuteNonQuery(queryBuilder);
    }

    #endregion
}