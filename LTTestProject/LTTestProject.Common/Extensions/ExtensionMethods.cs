using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTTestProject.Common.Extensions
{
    public static class ExtensionMethods
    {
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, System.Linq.Expressions.Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }
    }
}
