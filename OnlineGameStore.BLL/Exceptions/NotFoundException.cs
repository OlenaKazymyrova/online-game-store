namespace OnlineGameStore.BLL.Exceptions;

public class NotFoundException : HttpException
{
    public NotFoundException(string message, Exception? inner = null, string? traceId = null)
        : base(message, 404, inner, traceId) { }
}