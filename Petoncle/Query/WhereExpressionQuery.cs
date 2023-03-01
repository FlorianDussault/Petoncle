using System.Linq.Expressions;

namespace PetoncleDb;

internal sealed class WhereExpressionQuery : IWhereQuery
{
    private readonly Expression _expression;

    public bool HasValue => _expression != null;

    public WhereExpressionQuery(Expression expression) => _expression = expression;

    public void Build(ref QueryBuilder queryBuilder) => queryBuilder.Append(_expression);
}