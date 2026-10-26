using Domain.Common;
using Domain.Enums;
using System.Text.Json;

namespace OnlineShop.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unhandled exception while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await WriteErrorResponseAsync(context, exception);
        }
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        Exception exception)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        var message = context.RequestServices
            .GetRequiredService<IWebHostEnvironment>()
            .IsDevelopment()
                ? exception.Message
                : ".خطای داخلی سرور رخ داده است";

        var result = new ServiceResult<object>
        {
            StatusCode = ResultStatusCode.InternalServerError,
            Message = message
        };

        context.Response.Clear();
        context.Response.StatusCode = (int)ResultStatusCode.InternalServerError;
        context.Response.ContentType = "application/json; charset=utf-8";

        await context.Response.WriteAsJsonAsync(result);
    }
}
