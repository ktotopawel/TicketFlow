namespace TicketFlow.Api.DTOs.V2;

public record PaginationMetadata(
    int Page,
    int PageSize,
    int TotalPages,
    int TotalItems
);