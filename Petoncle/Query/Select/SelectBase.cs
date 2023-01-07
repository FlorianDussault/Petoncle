namespace PetoncleDb;

internal abstract class SelectBase : QueryBase
{
    protected IWhereQuery WhereQuery { get; private set; }
    
    protected SelectBase(PObject pObject) : base(pObject)
    {
    }

    public void SetWhereQuery(IWhereQuery whereExpressionQuery) => WhereQuery = whereExpressionQuery;

    // public abstract override void Build(ref QueryBuilder queryBuilder);
    protected internal abstract void BuildCount(ref QueryBuilder queryBuilder);
}