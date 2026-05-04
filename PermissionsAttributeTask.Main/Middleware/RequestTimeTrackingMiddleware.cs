using System.Diagnostics;

namespace PermissionsAttributeTask.Main.Middleware;

public class RequestTimeTrackingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimeTrackingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<RequestTimeTrackingMiddleware> logger)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            stopwatch.Stop();
            logger.LogError($"{context.Request.Method} {context.Request.Path}{context.Request.QueryString.Value} " +
                            $"has thrown an error {e} after {stopwatch.ElapsedMilliseconds}ms");
            throw;
        }
        stopwatch.Stop();
        logger.LogInformation($"{context.Request.Method} {context.Request.Path}{context.Request.QueryString.Value} " +
                              $"took {stopwatch.ElapsedMilliseconds}ms");

    }
}