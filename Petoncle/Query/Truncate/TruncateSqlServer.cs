namespace PetoncleDb;

internal class TruncateSqlServer : TruncateBase
{
    public TruncateSqlServer(PObject pObject) : base(pObject)
    {
 
    
    }
    
    public override void Build(ref QueryBuilder queryBuilder)
    {
        queryBuilder.Append("TRUNCATE TABLE " + PObject.FullTableName);
    }

  
}