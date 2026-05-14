using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Api.Data;
using TicketFlow.Api.DTOs;
using TicketFlow.Api.DTOs.V2;
using TicketFlow.Api.Entities;
using TicketFlow.Api.Exceptions;
using TicketFlow.Api.Extensions;
using TicketFlow.Api.Mappers.V2;

namespace TicketFlow.Api.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class EventsController(AppDbContext db, EventMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<EventListResponse>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = db.Events;

        int totalCount = await query.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var events = await db.Events
            .OrderBy(e => e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => mapper.MapToResponse(e))
            .ToListAsync();

        var links = new PaginationLinks(
            FirstPage: Url.Action(nameof(GetAll), new { page = 1, pageSize }),
            LastPage: Url.Action(nameof(GetAll), new { page = totalPages, pageSize }),
            NextPage: page < totalPages ? Url.Action(nameof(GetAll), new { page = page + 1, pageSize }) : null,
            PreviousPage: page > 1 ? Url.Action(nameof(GetAll), new { page = page - 1, pageSize }) : null,
            SelfPage: Url.Action(nameof(GetAll), new { page, pageSize })
        );

        var metadata = new PaginationMetadata(page, pageSize, totalPages, totalCount);

        var res = new EventListResponse(events, metadata, links);

        return Ok(res);
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

        return CreatedAtAction(nameof(GetById), new { version = "2.0"  ,res.Id }, res);
    }
}