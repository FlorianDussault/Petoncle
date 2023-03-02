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
                yield return (T) obj;
            yield break;
        }

        if (!dataReader.Read()) yield break;

        List<(int Index, ColumnDefinition ColumnDefinition)> colu = new List<(int, ColumnDefinition)>();
        ReadOnlySpan<ColumnDefinition> objectColumns = _pObject.Columns;
        for (int i = 0; i < dataReader.FieldCount; i++)
        {
            string fieldName = dataReader.GetName(i);
            for (int j = 0; j < objectColumns.Length; j++)
                if (string.Equals(objectColumns[j].ColumnName, fieldName, StringComparison.InvariantCulture))
                    colu.Add(new ValueTuple<int, ColumnDefinition>(i, objectColumns[j]));
        }

        do
        {
            T obj = _pObject.CreateInstance<T>();
            for (int i = 0; i < colu.Count; i++)
            {
                object value = dataReader.GetValue(colu[i].Index);
                if (value is not DBNull)
                    colu[i].ColumnDefinition.SetValue(obj,  value);
            }
            yield return obj;
        } while (dataReader.Read());
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

    public IPetoncleEnumerable<T> OrderByAsc(Expression<Func<T, object>> orderByExpression)
    {
        _selectBase.SetOrderBy(new OrderByExpressionQuery(orderByExpression, OrderByDirection.Asc));
        return this;
    }

    public IPetoncleEnumerable<T> OrderByDesc(Expression<Func<T, object>> orderByExpression)
    {
        _selectBase.SetOrderBy(new OrderByExpressionQuery(orderByExpression, OrderByDirection.Desc));
        return this;
    }
    
    public IPetoncleEnumerable<T> OrderByAsc(params string[] columns)
    {
        foreach (string column in columns)
            _selectBase.SetOrderBy(new OrderBySqlQuery(column, OrderByDirection.Asc));
        return this;
    }

    public IPetoncleEnumerable<T> OrderByDesc(params string[] columns)
    {
        foreach (string column in columns)
            _selectBase.SetOrderBy(new OrderBySqlQuery(column, OrderByDirection.Desc));
        return this;
    }

    public IPetoncleEnumerable<T> Top(int top)
    {
        _selectBase.SetTop(new TopQuery(top));
        return this;
    }

    public IPetoncleEnumerable<T> Columns(params Expression<Func<T, object>>[] columnsExpression)
    {
        foreach (Expression<Func<T,object>> expression in columnsExpression)
            _selectBase.SetColumns(new ColumnExpressionQuery(expression));
        return this;
    }

    public IPetoncleEnumerable<T> Columns(params string[] columns)
    {
        foreach (string column in columns)
            _selectBase.SetColumns(new ColumnSqlQuery(column));
        return this;
    }
}