using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Borlay.DB.Search
{
    public static class FilterExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> source, object filter)
        {
            source = source.Where(filter);

            if (filter is IHasOrder hasOrder)
                source = source.OrderBy(hasOrder);
            else if (filter is IOrderBy order)
                source = source.OrderBy(order);

            if (filter is ISkipTake skipTake)
                source = source.SkipTake(skipTake);


            return source;
        }

        public static DataResult<T> Select<T>(this IQueryable<T> source, object filter)
        {
            source = source.Where(filter);

            var result = new DataResult<T>()
            {
                Total = source.Count(),
            };

            if (filter is IHasOrder hasOrder)
                source = source.OrderBy(hasOrder);
            else if (filter is IOrderBy order)
                source = source.OrderBy(order);

            if (filter is ISkipTake skipTake)
            {
                result.Skip = skipTake.Skip ?? 0;
                result.Take = skipTake.Take;

                source = source.SkipTake(skipTake);
            }

            result.Data = source.ToArray();

            return result;
        }

        public static DataResult<TResult> Convert<T, TResult>(this DataResult<T> data, Func<T, TResult> converter)
        {
            return new DataResult<TResult>()
            {
                Data = data?.Data?.Select(t => converter(t)).ToArray(),
                Skip = data?.Skip ?? 0,
                Take = data?.Take,
                Total = data?.Total ?? 0
            };
        }

        public static DataResult<TResult> Select<T, TResult>(this IQueryable<T> source, object filter, Func<T, TResult> converter)
        {
            source = source.Where(filter);

            var result = new DataResult<TResult>()
            {
                Total = source.Count(),
            };

            if (filter is IHasOrder hasOrder)
                source = source.OrderBy(hasOrder);
            else if (filter is IOrderBy order)
                source = source.OrderBy(order);

            if (filter is ISkipTake skipTake)
            {
                result.Skip = skipTake.Skip ?? 0;
                result.Take = skipTake.Take;

                source = source.SkipTake(skipTake);
            }

            var tables = source.ToArray();
            result.Data = tables.Select(t => converter(t)).ToArray();

            return result;
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, object filter)
        {

            var type = filter.GetType();
            var properties = type.GetRuntimeProperties().ToArray();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FilterAttribute>(true);
                if (attribute == null) continue;

                var value = property.GetValue(filter);
                if (value == null) continue;

                if (property.PropertyType.IsArray && attribute.Operation == Operation.Equals && attribute.Conjunction == Conjunction.Any)
                    source = source.Contains(attribute.Column, value);
                else
                {
                    if (property.PropertyType.IsArray)
                        throw new NotSupportedException($"Arrays are supported only with operation equals and conjunction any");

                    source = source.Where(attribute.Column, value, attribute.Operation);
                }
            }

            return source;
        }

        public static IQueryable<T> SkipTake<T>(this IQueryable<T> source, ISkipTake skipTake)
        {
            if (skipTake.Skip.HasValue)
                source = source.Skip(skipTake.Skip.Value);

            if (skipTake.Take.HasValue)
                source = source.Take(skipTake.Take.Value);

            return source;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IOrderBy order)
        {
            return source.OrderBy(order.OrderColumn, order.OrderDirection);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IHasOrder order)
        {
            if (order.Order != null)
                return source.OrderBy(order.Order.OrderColumn, order.Order.OrderDirection);

            return source;
        }
    }
}
