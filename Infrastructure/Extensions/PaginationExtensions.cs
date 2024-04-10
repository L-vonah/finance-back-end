using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            var totalItems = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).TagWithCallSite().ToListAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new PaginatedList<T>
            {
                Items = items,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Page = pageIndex
            };
        }

        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, PageFilter filter) where T : BaseEntity
        {
            filter.Validate(typeof(T));
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, filter.SortBy);
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

                source = filter.SortOrder == SortOrder.Descending
                    ? source.OrderByDescending(lambda)
                    : source.OrderBy(lambda);
            }
            else
            {
                source = filter.SortOrder == SortOrder.Descending
                    ? source.OrderByDescending(x => x.Id)
                    : source.OrderBy(x => x.Id);
            }

            return await source.ToPaginatedListAsync(filter.Page, filter.PageSize);
        }
    }
}
