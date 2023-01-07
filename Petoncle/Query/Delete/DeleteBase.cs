namespace PetoncleDb;

internal abstract class DeleteBase : QueryBase
{
    protected readonly object @Object;
    protected IWhereQuery WhereQuery { get; private set; }
    
    protected DeleteBase(PObject pObject, object obj) : base(pObject)
    {
    }

    public void SetWhereQuery(IWhereQuery whereExpressionQuery)
    {
        WhereQuery = whereExpressionQuery;
    }
}