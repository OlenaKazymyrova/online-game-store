namespace OnlineGameStore.BLL.Exceptions;

public class ValidationException : HttpException
{
    public ValidationException(string message, string? traceId = null)
        : base(message, 422, traceId) { }
}