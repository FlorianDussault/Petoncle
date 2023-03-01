namespace PetoncleDb;

internal class OrderBySqlQuery : IOrderByQuery
{
    private readonly string _column;
    private readonly OrderByDirection _orderDirection;

    public OrderBySqlQuery(string column, OrderByDirection orderDirection)
    {
        _column = column;
        _orderDirection = orderDirection;
    }

    public void Build(ref QueryBuilder queryBuilder)
    {
        queryBuilder.Append(_column + " " + (_orderDirection == OrderByDirection.Asc ? " ASC " : " DESC "));
    }
}