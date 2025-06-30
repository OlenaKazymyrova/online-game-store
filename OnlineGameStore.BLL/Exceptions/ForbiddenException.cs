namespace OnlineGameStore.BLL.Exceptions;

public class ForbiddenException : HttpException
{
    public ForbiddenException(string message, Exception? inner = null, string? traceId = null)
        : base(message, 403, inner, traceId) { }
}