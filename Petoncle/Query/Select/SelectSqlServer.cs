namespace PetoncleDb;

internal class SelectSqlServer : SelectBase
{
    public SelectSqlServer(PObject pObject) : base(pObject)
    {
 
    
    }
    
    public override void Build(ref QueryBuilder queryBuilder)
    {
        queryBuilder.Append("SELECT ");

        #region Columns
        if (PObject.ObjectType == ObjectType.Dynamic)
        {
            queryBuilder.Append("*");
        }
        else
        {
            for (int i = 0; i < PObject.Columns.Length; i++)
            {
                queryBuilder.Append(PObject.Columns[i].SqlColumnName + ",");
            }
            queryBuilder.RemoveLastChar();
        }

       
        #endregion
        
        queryBuilder.Append(" FROM " + PObject.FullTableName);
        
        #region WHERE

        if (WhereQuery != null)
        {
            queryBuilder.Append(" WHERE ");
            WhereQuery.Build(ref queryBuilder);
        }

        #endregion
        
    }

  
}