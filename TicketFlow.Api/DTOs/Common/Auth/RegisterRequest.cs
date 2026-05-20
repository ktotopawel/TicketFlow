namespace TicketFlow.Api.DTOs.Common.Auth;

public record RegisterRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}