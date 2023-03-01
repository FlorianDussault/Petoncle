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
    public IPetoncleEnumerable<T> Select<T>() => Select<T>(null, null, null, null);
    public IPetoncleEnumerable<T> Select<T>(string tableName) => Select<T>(null, tableName, null, null);
    public IPetoncleEnumerable<dynamic> Select(string tableName) => Select<dynamic>(null, tableName, null, null);

    public IPetoncleEnumerable<T> Select<T>(Expression<Func<T, bool>> where) => Select<T>(null, null, where, null);

    public IPetoncleEnumerable<T> Select<T>(Sql whereSql) => Select<T>(null, null, null, whereSql);

    public IPetoncleEnumerable<T> Select<T>(string tableName, Expression<Func<T, bool>> where) => Select<T>(null, tableName, where, null);
    
    public IPetoncleEnumerable<T> Select<T>(string tableName, Sql whereSql) => Select<T>(null, tableName, null, whereSql);
    
    public IPetoncleEnumerable<T> Select<T>(string schemaName, string tableName, Sql whereSql) => Select<T>(schemaName, tableName, null, whereSql);
    
    private IPetoncleEnumerable<T> Select<T>(string schemaName, string tableName, Expression whereExpression, Sql whereSql)
    {
        // TODO: Check performances
        Type type = typeof(T);
        if (tableName == null && type == typeof(object))
            throw new PetoncleException($"You cannot call the {nameof(Select)} method with a dynamic type without a table name in argument");
        
        Petoncle.ObjectManager.PrepareDbObject(typeof(T), schemaName, tableName, out PObject pObject);

        if (pObject == null)
            throw new PetoncleException($"Unknown object type: {typeof(T).FullName}");
        
        return new PetoncleEnumerable<T>(Connection, pObject, whereExpression, whereSql);
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
    
    #region Update

    public int Update(object obj) => Update(obj.GetType(), null, null, obj, null, null);
    public int Update(string tableName, object obj) => Update(obj.GetType(), null, tableName, obj, null, null);
    public int Update(string schemaName, string tableName, object obj) => Update(obj.GetType(), schemaName, tableName, obj, null, null);
    public int Update<T>(object obj, Expression<Func<T, bool>> whereExpression) => Update(obj.GetType(), null, null, obj, whereExpression, null);
    public int Update<T>(string tableName, object obj, Expression<Func<T, bool>> whereExpression) => Update(obj.GetType(), null, tableName, obj, whereExpression, null);
    public int Update<T>(string schemaName, string tableName, object obj, Expression<Func<T, bool>> whereExpression) => Update(obj.GetType(), schemaName, tableName, obj, whereExpression, null);
    
    public int Update<T>(T obj, Sql whereSql) => Update(obj.GetType(), null, null, obj, null, whereSql);
    public int Update<T>(string tableName, T obj,  Sql whereSql) => Update(obj.GetType(), null, tableName, obj, null, whereSql);
    public int Update<T>(string schema, string tableName, T obj,  Sql whereSql) => Update(obj.GetType(), schema, tableName, obj, null, whereSql);
    private int Update(Type type, string schemaName, string tableName, object obj, Expression whereExpression, Sql whereSql)
    {
        if (tableName == null && type == typeof(object))
            throw new PetoncleException($"You cannot call the {nameof(Update)} method with a dynamic type without a table name in argument");
        
        Petoncle.ObjectManager.PrepareDbObject(type, schemaName, tableName, out PObject pObject);
        pObject ??= new PObject(schemaName, tableName, null);
        
        UpdateBase updateBase = QueryFactory.Update(Connection, pObject, obj);
        QueryBuilder queryBuilder = new(pObject, Connection.DatabaseType);
        
        if (whereExpression != null) updateBase.SetWhereQuery(new WhereExpressionQuery(whereExpression));
        else if (whereSql != null) updateBase.SetWhereQuery(new WhereSqlQuery(whereSql));
        updateBase.Build(ref queryBuilder);
        
        using SqlClient sqlClient = new(Connection);
        return sqlClient.ExecuteNonQuery(queryBuilder);
    }
    #endregion

    #region Delete
    public int Delete<T>() => Delete(typeof(T), null, null, null, null, null);
    public int Delete(string tableName) => Delete(null, null, tableName, null, null, null);
    public int Delete(string schemaName, string tableName) => Delete(null, schemaName, tableName, null, null, null);
    public int Delete(object obj) => Delete(obj.GetType(), null, null, obj, null, null);
    public int Delete<T>(Expression<Func<T, bool>> whereExpression) => Delete(typeof(T), null, null, null, whereExpression, null);
    public int Delete<T>(string tableName, Expression<Func<T, bool>> whereExpression) => Delete(typeof(T), null, tableName, null, whereExpression, null);
    public int Delete<T>(string schemaName, string tableName, Expression<Func<T, bool>> whereExpression) => Delete(typeof(T), schemaName, tableName,null, whereExpression, null);
    public int Delete<T>(Sql whereSql) => Delete(typeof(T), null, null, null, null, whereSql);
    public int Delete(string tableName, Sql whereSql) => Delete(null, null, tableName, null, null, whereSql);
    public int Delete(string schemaName, string tableName, Sql whereSql) => Delete(null, schemaName, tableName, null, null, whereSql);

    private int Delete(Type type, string schemaName, string tableName, object obj, Expression whereExpression, Sql whereSql)
    {
        if (tableName == null && type == typeof(object))
            throw new PetoncleException($"You cannot call the {nameof(Delete)} method with a dynamic type without a table name in argument");
        
        Petoncle.ObjectManager.PrepareDbObject(type, schemaName, tableName, out PObject pObject);
        pObject ??= new PObject(schemaName, tableName, null);
        //     throw new PetoncleException($"Unknown object type: {type.FullName}");
        
        DeleteBase insertBase = QueryFactory.Delete(Connection, pObject, obj);
        QueryBuilder queryBuilder = new(pObject, Connection.DatabaseType);
        
        if (whereExpression != null) insertBase.SetWhereQuery(new WhereExpressionQuery(whereExpression));
        else if (whereSql != null) insertBase.SetWhereQuery(new WhereSqlQuery(whereSql));
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