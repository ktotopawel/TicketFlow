namespace TicketFlow.Api.DTOs.V2;

public record EventListResponse(
    List<EventResponse> Events,
    PaginationMetadata Pagination,
    PaginationLinks Links
);