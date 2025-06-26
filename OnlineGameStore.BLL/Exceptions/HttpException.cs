using System.Diagnostics;

namespace OnlineGameStore.BLL.Exceptions;

public abstract class HttpException : Exception
{
    public int StatusCode { get; }
    public string? TraceId { get; }

    protected HttpException(string message, int statusCode, string? traceId = null)
        : base(message)
    {
        StatusCode = statusCode;
        TraceId = traceId ?? Activity.Current?.Id;
    }
}