namespace PetoncleDb;

internal abstract class InsertBase : QueryBase
{
    protected readonly object @Object;
    protected InsertBase(PObject pObject, object obj) : base(pObject)
    {
        @Object = obj;
    }
}