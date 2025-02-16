using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Models;
using System.Linq.Expressions;

namespace PetFamily.Application.Extensions
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
    }
}
