namespace OnlineGameStore.BLL.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException(string message, Exception? inner = null, string? traceId = null)
        : base(message, 400, inner, traceId) { }
}