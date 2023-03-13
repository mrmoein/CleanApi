using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace CleanApi.Application.Common.Tools;

public static class DataExtensions
{
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, string clause,
        object?[] parameters)
    {
        return condition ? source.Where(clause, parameters) : source;
    }

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition,
        Expression<Func<TSource, bool>> expression)
    {
        return condition ? source.Where(expression) : source;
    }

    public static IQueryable<TSource> OrderByIf<TSource>(this IQueryable<TSource> source, bool condition,
        string orderBy)
    {
        return condition ? source.OrderBy(orderBy) : source;
    }

    public static IEnumerable<TSource> OrderByIf<TSource>(this IEnumerable<TSource> source, bool condition,
        Func<TSource, bool> expression)
    {
        return condition ? source.OrderBy(expression) : source;
    }

    public static bool None<TSource>(this IEnumerable<TSource> source)
    {
        return !source.Any();
    }

    public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return !source.Any(predicate);
    }
}