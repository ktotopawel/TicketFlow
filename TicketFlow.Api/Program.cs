using Microsoft.EntityFrameworkCore;
using TicketFlow.Api.Data;
using TicketFlow.Api.Extensions;

namespace TicketFlow.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDatabaseConfiguration(builder.Configuration);

        var app = builder.Build();
        app.Run();
    }
}