namespace OnlineGameStore.BLL.Exceptions;

public class InternalErrorException : HttpException
{
    public InternalErrorException(string message, string? traceId = null)
        : base(message, 500, traceId) { }
}