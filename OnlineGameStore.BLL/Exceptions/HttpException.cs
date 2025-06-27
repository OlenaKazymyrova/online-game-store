using System.Diagnostics;

namespace OnlineGameStore.BLL.Exceptions;

public abstract class HttpException : Exception
{
    public int StatusCode { get; }
    public string? TraceId { get; }

    protected HttpException(string message, int statusCode, Exception? inner = null, string? traceId = null)
        : base(message, inner)
    {
        StatusCode = statusCode;
        TraceId = traceId ?? Activity.Current?.Id;
    }
}