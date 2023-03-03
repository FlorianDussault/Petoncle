using System.Linq.Expressions;

namespace PetoncleDb;

internal class GroupByExpressionQuery : IGroupByQuery
{
    private readonly Expression _groupByExpression;

    public GroupByExpressionQuery(Expression groupByExpression) => _groupByExpression = groupByExpression;

    public void Build(ref QueryBuilder queryBuilder) => queryBuilder.Append(_groupByExpression);
}