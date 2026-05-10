using Microsoft.EntityFrameworkCore;
using TicketFlow.Api.Entities;

namespace TicketFlow.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Event>  Events { get; set; }
}