namespace TicketFlow.Api.Entities;

public record Event
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required string Location { get; set; }
    public required int AvailableTickets { get; set; }
    public required DateTime CreatedAt { get; set; }
}