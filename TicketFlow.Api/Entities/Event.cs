namespace TicketFlow.Api.Entities;

public record Event
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required string Location { get; init; }
    public required int AvailableTickets { get; init; }
    public required DateTime CreatedAt { get; init; }
}