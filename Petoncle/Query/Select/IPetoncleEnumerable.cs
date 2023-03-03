using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PetoncleDb;

public interface IPetoncleEnumerable<T> : IEnumerable<T> {
    
    int Count { get; }

    IPetoncleEnumerable<T> OrderByAsc(Expression<Func<T, object>> orderByExpression);
    IPetoncleEnumerable<T> OrderByDesc(Expression<Func<T, object>> orderByExpression);
    IPetoncleEnumerable<T> OrderByAsc(params string[] columns);
    IPetoncleEnumerable<T> OrderByDesc(params string[] columns);
    IPetoncleEnumerable<T> Top(int top);
    IPetoncleEnumerable<T> Columns(params Expression<Func<T, object>>[] columnsExpression);
    IPetoncleEnumerable<T> Columns(params string[] columns);
    IPetoncleEnumerable<T> GroupBy(params Expression<Func<T, object>>[] columnsExpression);
    IPetoncleEnumerable<T> GroupBy(params string[] columns);
}