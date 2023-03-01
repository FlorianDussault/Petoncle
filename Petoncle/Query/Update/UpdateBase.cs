namespace PetoncleDb;

internal abstract class UpdateBase : QueryBase
{
    protected readonly object @Object;
    protected IWhereQuery WhereQuery { get; private set; }
    
    protected UpdateBase(PObject pObject, object obj) : base(pObject)
    {
        @Object = obj; 
    }

    public void SetWhereQuery(IWhereQuery whereExpressionQuery)
    {
        WhereQuery = whereExpressionQuery;
    }
}