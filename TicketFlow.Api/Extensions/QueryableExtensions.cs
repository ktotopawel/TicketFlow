using Microsoft.EntityFrameworkCore;

namespace TicketFlow.Api.Extensions;

public static class QueryableExtensions
{
    public static async Task<T> FirstOrThrowAsync<T>(this IQueryable<T> query, Exception ex)
    {
        var result = await query.FirstOrDefaultAsync();
        return result ?? throw ex;
    }
}