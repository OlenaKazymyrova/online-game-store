namespace OnlineGameStore.BLL.Exceptions;

public class InternalErrorException : HttpException
{
    public InternalErrorException(string message, Exception? inner = null, string? traceId = null)
        : base(message, 500, inner, traceId) { }
}