namespace PetoncleDb;

internal sealed class InsertSqlServer : InsertBase
{
    public InsertSqlServer(PObject pObject, object obj) : base(pObject, obj)
    {
    
    
    }
    
    public override void Build(ref QueryBuilder queryBuilder)
    {
        queryBuilder.Append("INSERT INTO " + PObject.FullTableName);
        queryBuilder.Append(" (");
        foreach (ColumnDefinition columnDefinition in PObject.ColumnsCanWrite)
        {
            queryBuilder.Append(columnDefinition.SqlColumnName + ",");
        }
        queryBuilder.RemoveLastChar();
        queryBuilder.Append(") VALUES (");

        foreach (ColumnDefinition columnDefinition in PObject.ColumnsCanWrite)
        {
            queryBuilder.RegisterArgumentAndAppend(columnDefinition.GetValue(@Object));
            queryBuilder.Append(",");
        }
        queryBuilder.RemoveLastChar();
        queryBuilder.Append(")");

    }
}