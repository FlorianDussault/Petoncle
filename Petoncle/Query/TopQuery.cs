namespace PetoncleDb;

internal class TopQuery : IQuery
{
    private readonly int _top;

    public TopQuery(int top) => _top = top;

    public void Build(ref QueryBuilder queryBuilder) => queryBuilder.Append(" TOP " + _top + " ");
}