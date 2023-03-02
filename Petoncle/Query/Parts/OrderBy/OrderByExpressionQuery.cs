using System.Linq.Expressions;

namespace PetoncleDb;

internal class OrderByExpressionQuery : IOrderByQuery
{
    private readonly Expression _orderByExpression;
    private readonly OrderByDirection _orderDirection;

    public OrderByExpressionQuery(Expression orderByExpression, OrderByDirection orderDirection)
    {
        _orderByExpression = orderByExpression;
        _orderDirection = orderDirection;
    }

    public void Build(ref QueryBuilder queryBuilder)
    {
        queryBuilder.Append(_orderByExpression);
        queryBuilder.Append(_orderDirection == OrderByDirection.Asc ? " ASC " : " DESC ");
    }
}