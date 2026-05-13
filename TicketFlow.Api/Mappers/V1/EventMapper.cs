using Riok.Mapperly.Abstractions;
using TicketFlow.Api.DTOs;
using TicketFlow.Api.Entities;

namespace TicketFlow.Api.Mappers.V1;

[Mapper]
public partial class EventMapper
{
    public partial EventResponse MapToResponse(Event e);
    
    public partial Event MapToEntity(CreateEventRequest req);
}