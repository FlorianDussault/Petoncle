namespace PetoncleDb;

internal sealed class UpdateSqlServer : UpdateBase
{
    public UpdateSqlServer(PObject pObject, object obj) : base(pObject, obj)
    {
    
    
    }
    
    public override void Build(ref QueryBuilder queryBuilder)
    {
        queryBuilder.Append("UPDATE " + PObject.FullTableName + " SET ");
       
        foreach (ColumnDefinition columnDefinition in PObject.ColumnsCanWrite)
        {
            queryBuilder.Append(columnDefinition.SqlColumnName + " = ");
            queryBuilder.RegisterArgumentAndAppend(columnDefinition.GetValue(@Object));
            queryBuilder.Append(",");
        }
        queryBuilder.RemoveLastChar();

        if (WhereQuery == null) return;
        
        queryBuilder.Append(" WHERE ");
        WhereQuery.Build(ref queryBuilder);
    }
}