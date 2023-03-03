﻿
using System.Collections.Generic;
using System.Data.Common;

namespace PetoncleDb;

internal abstract class SelectBase : QueryBase
{
    protected IWhereQuery WhereQuery { get; private set; }
    protected IList<IOrderByQuery> OrderByQueries { get; private set; } = new List<IOrderByQuery>();
    protected TopQuery TopQuery { get; private set; }
    protected IList<IColumnQuery> ColumnQueries { get; private set; } = new List<IColumnQuery>();

    protected IList<IGroupByQuery> GroupByQueries { get; private set; } = new List<IGroupByQuery>();
    protected SelectBase(PObject pObject) : base(pObject)
    {
    }

    public void SetWhereQuery(IWhereQuery whereExpressionQuery) => WhereQuery = whereExpressionQuery;

    public void SetOrderBy(IOrderByQuery orderByQuery) => OrderByQueries.Add(orderByQuery);

    public void SetTop(TopQuery topQuery) => TopQuery = topQuery;

    public void SetColumns(IColumnQuery columnQuery) => ColumnQueries.Add(columnQuery);

    public void SetGroupBy(IGroupByQuery groupByQuery) => GroupByQueries.Add(groupByQuery);
    
    protected internal abstract void BuildCount(ref QueryBuilder queryBuilder);
}