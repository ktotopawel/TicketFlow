using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TicketFlow.Api.Configuration;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription apiVersionDescription)
    {
        var info = new OpenApiInfo()
        {
            Title = "Ticket Flow API",
            Version = apiVersionDescription.ApiVersion.ToString(),
            Description = "API for managing tickets in the Ticket Flow system.",
        };

        if (apiVersionDescription.IsDeprecated)
        {
            info.Description += "** This API version has been deprecated. **";
        }

        return info;
    }
}