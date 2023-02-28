namespace PetoncleDb;

internal sealed class DeleteSqlServer : DeleteBase
{
    public DeleteSqlServer(PObject pObject, object obj) : base(pObject, obj)
    {
    
    
    }
    
    public override void Build(ref QueryBuilder queryBuilder)
    {
        queryBuilder.Append("DELETE FROM " + PObject.FullTableName);

        if (WhereQuery == null) return;
        
        queryBuilder.Append(" WHERE ");
        WhereQuery.Build(ref queryBuilder);
    }
}