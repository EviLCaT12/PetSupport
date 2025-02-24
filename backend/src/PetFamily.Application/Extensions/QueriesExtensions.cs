using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;

namespace PetFamily.Application.Extensions;

public static class QueriesExtensions
{
    public static async Task<PagedList<T>> ToPagedList<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var count = await source.CountAsync(ct);
        
        var items = await source.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        
        return new PagedList<T>
        {
            Items = items,
            PageSize = pageSize,
            Page = page,
            TotalCount = count
        };
    }
}