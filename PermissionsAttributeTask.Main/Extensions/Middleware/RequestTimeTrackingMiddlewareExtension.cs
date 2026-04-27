using PermissionsAttributeTask.Main.Middleware;

namespace PermissionsAttributeTask.Main.Extensions.Middleware;

public static class RequestTimeTrackingMiddlewareExtension
{
    public static IApplicationBuilder UseRequestsTiming(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTimeTrackingMiddleware>();
    }
}