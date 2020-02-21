using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Borlay.DB.Search
{
    public static class QueryableExtensions
    {
        private static MethodInfo stringContainsMethod = typeof(string).GetRuntimeMethod("Contains", new Type[] { typeof(string) });
        private static MethodInfo stringStartsWithMethod = typeof(string).GetRuntimeMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo stringEndsWithMethod = typeof(string).GetRuntimeMethod("EndsWith", new Type[] { typeof(string) });


        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, Direction direction)
        {
            if (string.IsNullOrWhiteSpace(ordering)) return source;

            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");

            var property = typeof(T).GetRuntimeProperties().FirstOrDefault(p => p.Name.ToLower() == ordering.ToLower());
            MemberExpression member = Expression.MakeMemberAccess(parameter, property);

            var orderByExp = Expression.Lambda(member, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable),
                                                             direction == Direction.Asc ? "OrderBy" : "OrderByDescending",
                                                             new[] { type, property.PropertyType }, source.Expression,
                                                             Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, string propertyName, object propertyValue, Operation operation = Operation.Equals)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");

            var equalExpression = GetExpression(parameter, propertyName, propertyValue, operation);
            var whereClause = Expression.Lambda<Func<T, bool>>(equalExpression, parameter);

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "Where",
                                                             new[] { type }, source.Expression,
                                                             whereClause);

            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> Contains<T>(this IQueryable<T> source, string propertyName, object propertyValue)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");

            MemberExpression member = Expression.Property(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);


            var elementType = propertyValue.GetType().GetElementType();
            var propertyType = propertyValue.GetType();
            var typeInfo = propertyType.GetTypeInfo();


            var filterContains = Expression.Call(
                typeof(Enumerable), "Contains", new[] { elementType },
                constant, member);


            var whereClause = Expression.Lambda(filterContains, parameter);


            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "Where",
                                                             new[] { type }, source.Expression,
                                                             whereClause);

            return source.Provider.CreateQuery<T>(resultExp);
        }

        private static Expression GetExpression(ParameterExpression param, string propertyName, object propertyValue, Operation operation)
        {
            MemberExpression member = Expression.Property(param, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);

            switch (operation)
            {
                case Operation.Equals:
                    return Expression.Equal(member, constant);

                case Operation.Greater:
                    return Expression.GreaterThan(member, constant);

                case Operation.GreaterOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Operation.Less:
                    return Expression.LessThan(member, constant);

                case Operation.LessOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Operation.Contains:
                    return Expression.Call(member, stringContainsMethod, constant);

                case Operation.StartsWith:
                    return Expression.Call(member, stringStartsWithMethod, constant);

                case Operation.EndsWith:
                    return Expression.Call(member, stringEndsWithMethod, constant);
            }

            throw new KeyNotFoundException($"Operation '{operation}' not found");
        }
    }
}
