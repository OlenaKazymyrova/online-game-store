namespace OnlineGameStore.BLL.DTOs.Tokens;

public class TokenResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
}