namespace OnlineGameStore.BLL.DTOs.Tokens;

public class RefreshTokenRequestDto
{
    public required Guid UserId { get; set; }
    public required string RefreshToken { get; set; }
}