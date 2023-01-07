using System;
using System.Linq;
using System.Linq.Expressions;

namespace PetoncleDb;

internal class ObjectDefinition
{
    public ObjectType ObjectType { get; }

    public Type Type { get; }
    public IDbTable Table { get; }

    /// <summary>
    /// All columns
    /// </summary>
    public ColumnDefinition[] Columns { get; }

    /// <summary>
    /// Primary Keys
    /// </summary>
    private readonly ColumnDefinition[] _primaryKeys;
    
    public ColumnDefinition[] ColumnsCanWrite { get; }
    
    public Span<ColumnDefinition> PrimaryKeys => _primaryKeys;

    private object _createInstance;    
    public ObjectDefinition(ObjectType objectType, Type type, IDbTable dbTable, ColumnDefinition[] columnDefinitions)
    {
        ObjectType = objectType;
        Type = type;
        Table = dbTable;
        Columns = columnDefinitions;
        _primaryKeys = columnDefinitions.Where(c => c.IsPrimary).ToArray();
        ColumnsCanWrite = columnDefinitions.Where(c => !c.IsReadOnly).ToArray();
    }


    internal PObject CreatePObject(string schema, string tableName)
    {
        
        return new PObject(schema, tableName, this);
    }

    internal ObjectDefinition SetAction<T>()
    {
        _createInstance = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
        return this;
    }

    internal T CreateInstance<T>() => ((Func<T>) _createInstance).Invoke();
}

internal enum ObjectType
{
    Petoncle,
    Anonymous,
    Dynamic
}

internal sealed class PObject
{
    public readonly ObjectDefinition _objectDefinition;

    public Type Type => _objectDefinition.Type;

    public ColumnDefinition[] Columns => _objectDefinition.Columns;

    public ColumnDefinition[] ColumnsCanWrite => _objectDefinition.ColumnsCanWrite;

    public T CreateInstance<T>() => _objectDefinition.CreateInstance<T>();
    
    public ObjectType ObjectType => _objectDefinition.ObjectType;
    
    public string FullTableName { get; } 
    
    public PObject(string schemaName, string tableName, ObjectDefinition objectDefinition)
    {
        _objectDefinition = objectDefinition;
        
        if (schemaName != null)
            FullTableName = "[" + schemaName + "].[";
        else if (objectDefinition.Table?.SchemaName != null)
            FullTableName = "[" + objectDefinition.Table.SchemaName + "].[";
        else FullTableName = "[";

        if (tableName != null)
            FullTableName += tableName + "]";
        else FullTableName += objectDefinition.Table?.TableName + "]";
    }

    public ColumnDefinition GetColumn(string memberName)
    {
        for (int i = 0; i < Columns.Length; i++)
        {
            if (Columns[i].Property.Name == memberName) return Columns[i];
        }

        return null;
    }
}
