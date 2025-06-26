namespace OnlineGameStore.BLL.Exceptions;

public class NotFoundException : HttpException
{
    public NotFoundException(string message, string? traceId = null)
        : base(message, 404, traceId) { }
}