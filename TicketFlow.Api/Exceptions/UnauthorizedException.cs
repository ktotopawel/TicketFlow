using System.Net;

namespace TicketFlow.Api.Exceptions;

public class UnauthorizedException(string message, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
    : AppException(message, statusCode)
{
}