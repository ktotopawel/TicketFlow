namespace TicketFlow.Api.DTOs.V2;

public record PaginationLinks(
    string? FirstPage,
    string? PreviousPage,
    string? NextPage,
    string? LastPage,
    string? SelfPage
);