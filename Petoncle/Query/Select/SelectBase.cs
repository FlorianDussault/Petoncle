using System.Collections.Generic;

namespace PetoncleDb;

internal abstract class SelectBase : QueryBase
{
    protected IWhereQuery WhereQuery { get; private set; }
    protected IList<IOrderByQuery> OrderByQueries { get; private set; } = new List<IOrderByQuery>();
    protected TopQuery TopQuery { get; private set; }
    protected IList<IColumnQuery> ColumnQueries { get; private set; } = new List<IColumnQuery>();
    
    protected SelectBase(PObject pObject) : base(pObject)
    {
    }

    public void SetWhereQuery(IWhereQuery whereExpressionQuery) => WhereQuery = whereExpressionQuery;

    public void SetOrderBy(IOrderByQuery orderByQuery) => OrderByQueries.Add(orderByQuery);

    public void SetTop(TopQuery topQuery) => TopQuery = topQuery;

    public void SetColumns(IColumnQuery columnQuery) => ColumnQueries.Add(columnQuery);
    
    protected internal abstract void BuildCount(ref QueryBuilder queryBuilder);
}