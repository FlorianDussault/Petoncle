using System;
using System.Linq;
using System.Reflection;

namespace PetoncleDb;

internal static class AttributeHelper
{
    internal static bool IsIDbTable(Attribute attribute, out IDbTable idbTable)
    {
        Type type = (Type) attribute.TypeId;
        if (type == typeof(DbTable) || type == typeof(DbTableSqlServer))
        {
            idbTable = (IDbTable)attribute;
            return true;
        }
        idbTable = null;
        return false;
    }

    internal static bool IsDbColumn(PropertyInfo propertyInfo, out DbColumn dbColumn, out bool isPrimary, out bool isAutoIncrement, out bool isReadOnly)
    {
        dbColumn = null;
        isPrimary = false;
        isAutoIncrement = false;
        isReadOnly = !propertyInfo.CanWrite;
        
        if (!propertyInfo.CanRead) return false;
        
        dbColumn = propertyInfo.GetCustomAttributes<DbColumn>().FirstOrDefault();
        DbPrimary dbPrimary = propertyInfo.GetCustomAttributes<DbPrimary>().FirstOrDefault();
        if (dbColumn == null && dbPrimary == null)
            return false;
        dbColumn ??= dbPrimary;
        
        isPrimary = dbPrimary != null;
        isReadOnly = isAutoIncrement = propertyInfo.GetCustomAttributes<DbAutoIncrement>().Any();
        if (!isReadOnly)
        {
            isReadOnly = propertyInfo.GetCustomAttributes<DbReadOnly>().Any();
        }
        return true;
    }
}