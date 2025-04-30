using Microsoft.EntityFrameworkCore;
using PWAApi.ApiService.Models;

namespace PWAApi.ApiService.Extensions
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Converts an <see cref="IQueryable{T}"/> source into a paginated result asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The queryable source to paginate.</param>
        /// <param name="page">The current page number (1-based). Defaults to 1 if less than 1.</param>
        /// <param name="pageSize">The number of items per page. Defaults to 10 if less than 1.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A <see cref="PaginatedResult{T}"/> containing the paginated items, current page, page size, total pages, and total items.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>

        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
            this IQueryable<T> source, 
            int page, 
            int pageSize, 
            CancellationToken cancellationToken = default)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var totalItems = await source.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<T>
            {
                Items = items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems
            };
        }
    }
}
