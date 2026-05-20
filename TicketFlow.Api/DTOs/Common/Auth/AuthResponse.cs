using TicketFlow.Api.Entities;

namespace TicketFlow.Api.DTOs.Common.Auth;

public record AuthResponse
{
    public required string Token { get; init; }
    public required string Email { get; init; }
    public required Role Role { get; init; }
}