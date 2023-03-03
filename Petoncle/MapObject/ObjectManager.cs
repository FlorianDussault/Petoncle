using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;


namespace PetoncleDb;

internal class ObjectManager
{
    private ObjectDefinition[] Objects { get; set; }
    private ObjectDefinition DynamicObject { get; set; }
    private Collection<ObjectDefinition> _bufferList;
    
    public ObjectManager()
    {
        _bufferList = new Collection<ObjectDefinition>();
    }

    public void RegisterObject<T>()
    {
        if (typeof(T) == typeof(object))
            throw new PetoncleException($"Object {typeof(T).FullName} not supported.");
        
        Attribute[] attributes = Attribute.GetCustomAttributes(typeof(T));
        for (int i = 0; i < attributes.Length; i++)
        {
            if (!AttributeHelper.IsIDbTable(attributes[i], out IDbTable dbTable)) continue;
            _bufferList.Add(RegisterDbTable<T>(dbTable));
            return;
        }
        _bufferList.Add(RegisterAnonymous<T>());
    }
    
    private ObjectDefinition RegisterDbTable<T>(IDbTable dbTable)
    {
        List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
        PropertyInfo[] properties = typeof(T).GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            if (AttributeHelper.IsDbColumn(properties[i], out DbColumn dbColumn, out bool isPrimary, out bool isAutoIncrement, out bool isReadOnly))
            {
                columnDefinitions.Add(new ColumnDefinition(properties[i], dbColumn, isPrimary, isAutoIncrement, isReadOnly).SetActions<T>());
            }
        }
        if (columnDefinitions.Count == 0) throw new PetoncleException($"No properties found in object {typeof(T).Name}");
        return new ObjectDefinition(ObjectType.Petoncle ,typeof(T), dbTable, columnDefinitions.ToArray()).SetAction<T>();
    }
    
    private ObjectDefinition RegisterAnonymous<T>()
    {
        List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
        PropertyInfo[] properties = typeof(T).GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            if (!properties[i].CanRead) continue;
            columnDefinitions.Add(new ColumnDefinition(properties[i], new DbColumn(properties[i].Name), false, false, !properties[i].CanWrite).SetActions<T>());
        }

        if (columnDefinitions.Count == 0) throw new PetoncleException($"No properties found in object {typeof(T).Name}");
        return new ObjectDefinition(ObjectType.Anonymous, typeof(T), new DbTable(typeof(T).Name), columnDefinitions.ToArray()).SetAction<T>();
    }
    
    private ObjectDefinition RegisterDynamic(object obj)
    {
        List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
        PropertyInfo[] properties = obj.GetType().GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            if (!properties[i].CanRead) continue;
            columnDefinitions.Add(new ColumnDefinition(properties[i], new DbColumn(properties[i].Name), false, false, false));
        }
    
        if (columnDefinitions.Count == 0) throw new PetoncleException($"No properties found in dynamic object");
        return new ObjectDefinition(ObjectType.Dynamic, obj.GetType(), new DbTable(obj.GetType().Name), columnDefinitions.ToArray());
    }

    internal void CommitTables()
    {
        // Add dynamic
        DynamicObject = new ObjectDefinition(ObjectType.Dynamic, typeof(object), new DbTable(null),Array.Empty<ColumnDefinition>());
        _bufferList.Add(DynamicObject);
        Objects = _bufferList.ToArray();
    }

    public void PrepareDbObject(Type type, string schemaName, string tableName,  out PObject pObject) => PrepareDbObject(type, schemaName, tableName, null, out pObject);

    public void PrepareDbObject(Type type, string schemaName, string tableName, object obj,  out PObject pObject)
    {
        pObject = null;
        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i].Type != type) continue;
            pObject = Objects[i].CreatePObject(schemaName, tableName);
            return;
        }
        if (obj != null)
            pObject = RegisterDynamic(obj).CreatePObject(schemaName, tableName);
        return;
        
        if (obj != null && obj.GetType() == typeof(object))
        {
            
            // Dynamic
            
        }
    }
}
