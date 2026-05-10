using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Api.Data;
using TicketFlow.Api.DTOs;
using TicketFlow.Api.Entities;

namespace TicketFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetAll()
    {
        var events = await db.Events
            .Select(e =>
                new EventResponse(e.Id, e.Name, e.Description, e.StartDate, e.EndDate, e.Location, e.AvailableTickets))
            .ToListAsync();

        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponse>> GetById(int id)
    {
        var eventWithId = await db.Events
            .Where(e => e.Id == id)
            .Select(e =>
                new EventResponse(e.Id, e.Name, e.Description, e.StartDate, e.EndDate, e.Location, e.AvailableTickets))
            .FirstOrDefaultAsync();

        return Ok(eventWithId);
    }

    [HttpPost]
    public async Task<ActionResult<EventResponse>> Create(CreateEventRequest req)
    {
        var newEvent = new Event
        {
            Name = req.Name,
            Description = req.Description,
            StartDate = req.StartDate,
            EndDate = req.EndDate,
            Location = req.Location,
            AvailableTickets = req.AvailableTickets
        };

        db.Events.Add(newEvent);
        await db.SaveChangesAsync();

        var res = new EventResponse(newEvent.Id, newEvent.Name, newEvent.Description, newEvent.StartDate,
            newEvent.EndDate, newEvent.Location, newEvent.AvailableTickets);

        return CreatedAtAction(nameof(GetById), new { res.Id }, res);
    }
}