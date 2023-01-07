namespace PetoncleDb;

internal abstract class QueryBase : IQuery
{
    protected readonly PObject PObject;

    protected QueryBase(PObject pObject)
    {
        PObject = pObject;
    }
    
    public abstract void Build(ref QueryBuilder queryBuilder);
}