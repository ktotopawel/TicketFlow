using Riok.Mapperly.Abstractions;
using TicketFlow.Api.DTOs;
using TicketFlow.Api.DTOs.V2;
using TicketFlow.Api.Entities;

namespace TicketFlow.Api.Mappers.V2;

[Mapper]
public partial class EventMapper
{
    public partial EventResponse MapToResponse(Event e);
    
    public partial Event MapToEntity(CreateEventRequest req, DateTime createdAt);
}
