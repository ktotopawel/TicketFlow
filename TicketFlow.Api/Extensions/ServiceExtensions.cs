using Microsoft.EntityFrameworkCore;
using TicketFlow.Api.Data;

namespace TicketFlow.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connnectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = ServerVersion.AutoDetect(connnectionString);
        
        services.AddDbContext<AppDbContext>(options => options.UseMySql(connnectionString, serverVersion));
        
        return services;
    }

}