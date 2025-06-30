namespace OnlineGameStore.BLL.Exceptions;

public class ConflictException : HttpException
{
    public ConflictException(string message, Exception? inner = null, string? traceId = null)
        : base(message, 409, inner, traceId) { }
}