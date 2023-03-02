namespace PetoncleDb;

internal class SelectSqlServer : SelectBase
{
    public SelectSqlServer(PObject pObject) : base(pObject)
    {
 
    
    }
    
    public override void Build(ref QueryBuilder queryBuilder)
    {
        queryBuilder.Append("SELECT ");

        #region TOP
        TopQuery?.Build(ref queryBuilder);
        #endregion
        
        #region Columns
        if (PObject.ObjectType == ObjectType.Dynamic)
        {
            queryBuilder.Append("*");
        }
        else if (ColumnQueries.Count > 0)
        {
            for (int i = 0; i < ColumnQueries.Count; i++)
            {
                ColumnQueries[i].Build(ref queryBuilder);
                queryBuilder.Append(",");
            }
            queryBuilder.RemoveLastChar();
        }
        else
        {
            for (int i = 0; i < PObject.Columns.Length; i++)
                queryBuilder.Append(PObject.Columns[i].SqlColumnName + ",");
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
        
        #region ORDER BY

        if (OrderByQueries.Count > 0)
        {
            queryBuilder.Append(" ORDER BY ");
            for (int i = 0; i < OrderByQueries.Count; i++)
            {
                OrderByQueries[i].Build(ref queryBuilder);
                queryBuilder.Append(",");
            }
            queryBuilder.RemoveLastChar();
        }        
        #endregion
    }


    protected internal override void BuildCount(ref QueryBuilder queryBuilder)
    {
        queryBuilder.Append("SELECT COUNT(1) FROM " + PObject.FullTableName);

        if (WhereQuery == null) return;
        
        #region WHERE
        queryBuilder.Append(" WHERE ");
        WhereQuery.Build(ref queryBuilder);
        #endregion
    }
}