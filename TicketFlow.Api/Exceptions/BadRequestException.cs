using System.Net;

namespace TicketFlow.Api.Exceptions;

public sealed class BadRequestException(string message) : AppException(message, HttpStatusCode.BadRequest)
{
}