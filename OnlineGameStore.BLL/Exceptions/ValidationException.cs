namespace OnlineGameStore.BLL.Exceptions;

public class ValidationException : HttpException
{
    public ValidationException(string message, Exception? inner = null,string? traceId = null)
        : base(message, 422, inner, traceId) { }
}