using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace JS.Sample.Common.Extension
{
    public class ListExtension
    {
        public static Expression<Func<T, bool>> PredicateBuilder<T>(Expression<Func<T, bool>> expr1,
                                                 Expression<Func<T, bool>> expr2)
        {

            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static int getPageNumber(int pagenumber, int pagesize)
        {

            return (pagenumber - 1) * pagesize;
        }
    }
}
