using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, PageFilter filter)
        {
            filter.Validate(typeof(T));
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                var property = typeof(T).GetProperty(filter.SortBy);
                if (property != null)
                {
                    source = filter.SortOrder == SortOrder.Descending
                        ? source.OrderByDescending(x => property.GetValue(x, null))
                        : source.OrderBy(x => property.GetValue(x, null));
                }
            }
            else
            {
                source = filter.SortOrder == SortOrder.Descending
                    ? source.OrderByDescending(x => x)
                    : source.OrderBy(x => x);
            }

            return await source.ToPaginatedListAsync(filter.Page, filter.PageSize);
        }
    }
}
