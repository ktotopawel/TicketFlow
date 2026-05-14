namespace TicketFlow.Api.DTOs.V2;

public record EventResponse(
    int Id,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    int AvailableTickets);