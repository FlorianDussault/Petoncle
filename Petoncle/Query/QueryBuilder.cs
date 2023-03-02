using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;

namespace PetoncleDb;

internal sealed class QueryBuilder
{
    private readonly PObject _pObject;
    private readonly DatabaseType _databaseType;
    private readonly StringBuilder _stringBuilder = new();
    private readonly SqlArguments _sqlArguments = new();

    internal IReadOnlyCollection<SqlArgument> Arguments => _sqlArguments;

    internal QueryBuilder(PObject pObject, DatabaseType databaseType)
    {
        _pObject = pObject;
        _databaseType = databaseType;
    }

    public void Append(string sql) => _stringBuilder.Append(sql);

    public void Append(Expression expression)
    {
        LambdaParser.Parse(_databaseType, expression, _pObject, this, null);
    }
    
    public void Append(Sql sql)
    {
        sql.Append( this);
    }
    
    public string GetQuery() => _stringBuilder.ToString();

    public void RemoveLastChar() => _stringBuilder.Length--;

    public string RegisterArgument(object value) => _sqlArguments.Register(value);
    public void RegisterArgumentAndAppend(object value) => _stringBuilder.Append(_sqlArguments.Register(value));
}

internal sealed class SqlArguments : Collection<SqlArgument>
{
    internal string Register(object obj)
    {
        string argumentName = $"@_LZ_{Count}_LZ_@";
        Add(new SqlArgument(argumentName, obj));
        return argumentName;
    }

    public SqlArguments Add(string name, object obj)
    {
        Add(new SqlArgument(name, obj));
        return this;
    }
}

internal struct SqlArgument {
    public string Name { get;  }
    public object Value { get; }
    public SqlArgument(string name, object value)
    {
        Name = name;
        Value = value;
    }
}