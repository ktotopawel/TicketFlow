using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using TicketFlow.Api.Exceptions;

namespace TicketFlow.Api.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.ContentType = "text/plain";

                var endpoint = context.HttpContext.GetEndpoint();

                var rateLimitingAttribute = endpoint?.Metadata.GetMetadata<EnableRateLimitingAttribute>();
                var policyName = rateLimitingAttribute?.PolicyName;

                if (policyName == "DeprecatedAPIPolicy")
                {
                    await context.HttpContext.Response.WriteAsync(
                        "This API version is deprecated and has strict Rate Limits. Upgrade to a newer version to enjoy higher rate limits.",
                        cancellationToken: token);
                }
                else
                {
                    await context.HttpContext.Response.WriteAsync(
                        "Rate limit exceeded. Too many requests from your IP address. Please try again later.",
                        cancellationToken: token);
                }

                await context.HttpContext.Response.WriteAsync(
                    "Rate Limit exceeded. Please try again later.",
                    cancellationToken: token);
            };

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                if (ipAddress == "unknown")
                {
                    throw new BadRequestException("Unable to determine client IP address for rate limiting.");
                }

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ipAddress,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromSeconds(60),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }
                );
            });

            options.AddPolicy("DeprecatedAPIPolicy", httpContext =>
            {
                var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ipAddress,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromSeconds(10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });
        });

        return services;
    }
}