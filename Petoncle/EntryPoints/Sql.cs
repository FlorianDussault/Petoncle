using System.Reflection;

namespace PetoncleDb;

public sealed class Sql
{
    private readonly string _query;
    private readonly object _args;

    public Sql(string query)
    {
        _query = query;
    }
    
    public Sql(string query, object args)
    {
        _query = query;
        _args = args;
    }

    internal void Append(QueryBuilder queryBuilder)
    {
        string query = _query;

        if (_args != null)
        {
            foreach (PropertyInfo propertyInfo in _args.GetType().GetProperties())
            {
                string propertyName = $"@{propertyInfo.Name}";
                string newName = queryBuilder.RegisterArgument(propertyInfo.GetValue(_args));
                query = query.Replace(propertyName, newName);
            }
        }
        queryBuilder.Append(query);

    }
}