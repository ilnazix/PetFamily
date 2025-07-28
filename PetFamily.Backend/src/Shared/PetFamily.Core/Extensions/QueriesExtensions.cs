using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using PetFamily.Core.Models;

namespace PetFamily.Core.Extensions
{
    public static class QueriesExtensions
    {
        public static async Task<PagedList<T>> ToPagedList<T>(
            this IQueryable<T> source,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await source.CountAsync(cancellationToken);

            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var pagedList = new PagedList<T>()
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                Page = page
            };

            return pagedList;
        }

        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> source,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> Sort<T>(
            this IQueryable<T> source,
            string? orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return source;

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param)) continue;

                var propertyFromQueryName = param.Split(" ")[0];

                var objectProperty = propertyInfos
                    .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
                        StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null) continue;

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
            }

            var orderQuery = orderQueryBuilder
                   .ToString()
                   .TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
                return source;

            return source.OrderBy(orderQuery);
        }
    }
}
