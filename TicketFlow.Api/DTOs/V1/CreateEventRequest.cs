using System.ComponentModel.DataAnnotations;

namespace TicketFlow.Api.DTOs.V1;

public record CreateEventRequest(
    [Required] [StringLength(100)] string Name,
    [Required] string Description,
    [Required] DateTime StartDate,
    [Required] DateTime EndDate,
    [Required] string Location,
    [Required] [Range(1, 10000)] int AvailableTickets
    );