using Microsoft.EntityFrameworkCore;

namespace TicketFlow.Api.Entities;

[Index(nameof(Email), IsUnique = true)]
public record User
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required Role Role { get; init; }
    public string HashedPassword { get; init; }
    public DateTime CreatedAt { get; init; }
}