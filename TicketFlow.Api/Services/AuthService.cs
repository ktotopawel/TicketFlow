using TicketFlow.Api.DTOs.Common.Auth;
using TicketFlow.Api.Entities;
using TicketFlow.Api.Exceptions;

namespace TicketFlow.Api.Services;

public class AuthService(UserService userService, TokenService tokenService)
{
    public async Task<AuthResponse> Register(RegisterRequest req)
    {
        if (await userService.FindByEmailAsync(req.Email) != null)
        {
            throw new BadRequestException("Email already exists.");
        }

        var userId = Guid.NewGuid();

        var userToken = tokenService.GenerateToken(userId, req.Email, Role.User);

        await userService.CreateUser(userId, req);

        return new AuthResponse
        {
            Email = req.Email,
            Role = Role.User,
            Token = userToken
        };
    }

    public async Task<AuthResponse> Login(LoginRequest req)
    {
        var user = await userService.FindByEmailAsync(req.Email);

        if (user == null)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var verificationResult = userService.VerifyPassword(user, req.Password);

        if (!verificationResult)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var userToken = tokenService.GenerateToken(user.Id, user.Email, user.Role);

        return new AuthResponse
        {
            Email = user.Email,
            Role = user.Role,
            Token = userToken
        };
    }
}