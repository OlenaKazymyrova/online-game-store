namespace OnlineGameStore.BLL.DTOs;

public class LicenseResponseDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
}