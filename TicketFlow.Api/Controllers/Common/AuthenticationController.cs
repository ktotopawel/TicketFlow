using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketFlow.Api.Data;
using TicketFlow.Api.DTOs.Common.Auth;
using TicketFlow.Api.Entities;
using TicketFlow.Api.Exceptions;
using TicketFlow.Api.Services;

namespace TicketFlow.Api.Controllers.Common;

[ApiController]
[Route("/api/auth")]
[ApiVersionNeutral]
[AllowAnonymous]
public class AuthenticationController(AuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest req)
    {
        return await service.Register(req);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req)
    {
        return await service.Login(req);
    }
}