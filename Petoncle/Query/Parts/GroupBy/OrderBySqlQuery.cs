namespace PetoncleDb;

internal class GroupBySqlQuery : IGroupByQuery
{
    private readonly string _column;

    public GroupBySqlQuery(string column) => _column = column;

    public void Build(ref QueryBuilder queryBuilder) => queryBuilder.Append(_column);
}