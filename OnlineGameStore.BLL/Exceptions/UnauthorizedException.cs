namespace OnlineGameStore.BLL.Exceptions;

public class UnauthorizedException : HttpException
{
    public UnauthorizedException(string message, Exception? inner = null, string? traceId = null)
        : base(message, 401, inner, traceId) { }
}