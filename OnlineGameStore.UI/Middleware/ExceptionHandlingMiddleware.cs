using System.Diagnostics;
using Newtonsoft.Json;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Exceptions;

namespace OnlineGameStore.UI.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

        try
        {
            await next(context);
        }
        catch (HttpException ex)
        {
            _logger.LogError(ex, "Exception occurred. TraceId: {TraceId}", traceId);
            if (ex.InnerException != null)
            {
                _logger.LogError("Inner Exception: {InnerException}", ex.InnerException);
            }

            await HandleExceptionAsync(context, ex, traceId);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpException exception, string traceId)
    {
        var errorMessage = new ErrorMessage
        {
            Error = exception.Message,
            Type = exception.GetType().Name,
            TraceId = traceId
        };

        var response = JsonConvert.SerializeObject(errorMessage);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception.StatusCode;

        return context.Response.WriteAsync(response);
    }
}