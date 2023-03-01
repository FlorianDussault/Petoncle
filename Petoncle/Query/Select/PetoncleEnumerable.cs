using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;

namespace PetoncleDb;

internal sealed class PetoncleEnumerable<T> : IPetoncleEnumerable<T>
{
    private readonly Connection _connection;
    private readonly PObject _pObject;
    private readonly SelectBase _selectBase;

    public PetoncleEnumerable(Connection connection, PObject pObject, Expression whereExpression, Sql whereSql)
    {
        _connection = connection;
        _pObject = pObject;
        _selectBase = QueryFactory.Select(connection, _pObject);
        if (whereExpression != null)
            _selectBase.SetWhereQuery(new WhereExpressionQuery(whereExpression));
        else if (whereSql != null)
            _selectBase.SetWhereQuery(new WhereSqlQuery(whereSql));
    }

    public IEnumerator<T> GetEnumerator()
    {
        QueryBuilder queryBuilder = new (_pObject, _connection.DatabaseType);
        _selectBase.Build(ref queryBuilder);
        using SqlClient sqlClient = new(_connection);
        using IDataReader dataReader = sqlClient.ExecuteQuery(queryBuilder);

        if (_pObject.ObjectType == ObjectType.Dynamic)
        {
            foreach (object obj in ReadDynamic(dataReader))
            {
                yield return (T)obj;
            }

            yield break;
        }
        
        ColumnDefinition[] columns = _pObject.Columns;

        while (dataReader.Read())
        {
            T obj = _pObject.CreateInstance<T>();
            for (int j = 0; j < columns.Length; j++)
            {
                object value = dataReader.GetValue(j);
                if (value is not DBNull)
                    columns[j].SetValue(obj,  value);
            }
            yield return obj;
        }
        
    }
    private IEnumerable<T> ReadDynamic(IDataReader dataReader)
    {
        string[] columns = new string[dataReader.FieldCount];
        for (int i = 0; i < columns.Length; i++)
            columns[i] = dataReader.GetName(i);

        while (dataReader.Read())
        {
            IDictionary<string, object> obj = new ExpandoObject();
            for (int i = 0; i < columns.Length; i++)
            {
                object value = dataReader.GetValue(i);
                obj.Add(columns[i], value is not DBNull ? value : null);
            }
            yield return (T)obj;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count
    {
        get
        {
            QueryBuilder queryBuilder = new(_pObject, _connection.DatabaseType);
            _selectBase.BuildCount(ref queryBuilder);
            SqlClient sqlClient = new(_connection);
            return (int)sqlClient.ExecuteScalar(queryBuilder);
        }
    }
}
