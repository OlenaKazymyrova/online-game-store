namespace OnlineGameStore.BLL.Exceptions;

public class ConflictException : HttpException
{
    public ConflictException(string message, string? traceId = null)
        : base(message, 409, traceId) { }
}