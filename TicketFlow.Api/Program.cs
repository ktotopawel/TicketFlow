using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Api.Data;
using TicketFlow.Api.Entities;
using TicketFlow.Api.Extensions;
using TicketFlow.Api.Middleware;
using TicketFlow.Api.Services;
using V1 = TicketFlow.Api.Mappers.V1;
using V2 = TicketFlow.Api.Mappers.V2;

namespace TicketFlow.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddVersioningConfiguration();
        builder.Services.AddControllers();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddSecurityConfiguration(builder.Configuration);
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddSingleton<V1.EventMapper>();
        builder.Services.AddSingleton<V2.EventMapper>();
        builder.Services.AddRateLimitingConfiguration();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<AuthService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerConfiguration();
        }

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseRateLimiter();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}