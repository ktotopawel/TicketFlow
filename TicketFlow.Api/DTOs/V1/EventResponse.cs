namespace TicketFlow.Api.DTOs.V1;

public record EventResponse(
    int Id,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    int AvailableTickets);