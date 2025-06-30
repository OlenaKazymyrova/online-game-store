namespace OnlineGameStore.BLL.DTOs;

public class ErrorMessage
{
    public required string Error { get; set; }
    public required string Type { get; set; }
    public string? TraceId { get; set; }
}