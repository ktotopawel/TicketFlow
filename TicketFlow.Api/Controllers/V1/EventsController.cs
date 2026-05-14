using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Api.Data;
using TicketFlow.Api.DTOs;
using TicketFlow.Api.DTOs.V1;
using TicketFlow.Api.Entities;
using TicketFlow.Api.Exceptions;
using TicketFlow.Api.Extensions;
using TicketFlow.Api.Mappers.V1;

namespace TicketFlow.Api.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class EventsController(AppDbContext db, EventMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetAll()
    {
        var events = await db.Events
            .Select(e => mapper.MapToResponse(e))
            .ToListAsync();

        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponse>> GetById(int id)
    {
        var eventWithId = await db.Events
            .Where(e => e.Id == id)
            .Select(e => mapper.MapToResponse(e))
            .FirstOrThrowAsync(new ResourceNotFoundException("event", id));

        return Ok(eventWithId);
    }

    [HttpPost]
    public async Task<ActionResult<EventResponse>> Create(CreateEventRequest req)
    {
        var newEvent = mapper.MapToEntity(req, DateTime.UtcNow);

        db.Events.Add(newEvent);
        await db.SaveChangesAsync();

        var res = mapper.MapToResponse(newEvent);

        return CreatedAtAction(nameof(GetById), new { version = "1.0", res.Id }, res);
    }
}