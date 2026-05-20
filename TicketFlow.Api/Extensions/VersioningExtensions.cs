using Asp.Versioning;

namespace TicketFlow.Api.Extensions;

public static class VersioningExtensions
{
    public static IServiceCollection AddVersioningConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader()
                );
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}