namespace PetoncleDb;

internal sealed class WhereSqlQuery : IWhereQuery
{
    private readonly Sql _sql;

    public bool HasValue => _sql != null;

    public WhereSqlQuery(Sql whereSql) => _sql = whereSql;

    public void Build(ref QueryBuilder queryBuilder) => queryBuilder.Append(_sql);
}