using System.Linq.Expressions;

namespace PetoncleDb;

internal class ColumnExpressionQuery : IColumnQuery
{
    private readonly Expression _orderByExpression;

    public ColumnExpressionQuery(Expression orderByExpression) => _orderByExpression = orderByExpression;

    public void Build(ref QueryBuilder queryBuilder) => queryBuilder.Append(_orderByExpression);
}