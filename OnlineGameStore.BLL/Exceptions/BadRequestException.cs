namespace OnlineGameStore.BLL.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException(string message)
        : base(message, 400) { }
}