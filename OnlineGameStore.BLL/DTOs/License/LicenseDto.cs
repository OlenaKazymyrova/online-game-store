namespace OnlineGameStore.BLL.DTOs;

public class LicenseDto
{
    public string Description { get; set; }

    public decimal Cost { get; set; }

    public Guid GameId { get; set; }
}
