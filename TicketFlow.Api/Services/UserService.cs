using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Api.Data;
using TicketFlow.Api.DTOs.Common.Auth;
using TicketFlow.Api.Entities;

namespace TicketFlow.Api.Services;

public class UserService(IPasswordHasher<User> hasher, AppDbContext db)
{
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await db.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateUser(Guid userId, RegisterRequest request)
    {
        var user = new User
        {
            Id = userId,
            Email = request.Email,
            Role = Role.User,
            CreatedAt = DateTime.UtcNow
        };

        user = user with { HashedPassword = hasher.HashPassword(user, request.Password) };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return user;
    }

    public bool VerifyPassword(User user, string password)
    {
        var result = hasher.VerifyHashedPassword(user, user.HashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}