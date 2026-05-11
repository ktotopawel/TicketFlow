using System.Net;

namespace TicketFlow.Api.Exceptions;

public sealed class ResourceNotFoundException(string resourceName, object key)
    : AppException($"{resourceName} with key '{key}' was not found.", HttpStatusCode.NotFound)
{
}