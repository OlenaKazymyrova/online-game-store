namespace OnlineGameStore.BLL.DTOs;

public class PlatformResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Guid> GameIds { get; set; } = new();
}