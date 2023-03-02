namespace PetoncleDb;

internal class ColumnSqlQuery : IColumnQuery
{
    private readonly string _column;

    public ColumnSqlQuery(string column) => _column = column;

    public void Build(ref QueryBuilder queryBuilder) => queryBuilder.Append(_column);
}