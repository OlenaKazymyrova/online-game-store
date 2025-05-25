namespace OnlineGameStore.BLL.DTOs;

public class PlatformDto
{
    public string Name { get; set; }
    public List<Guid> GameIds { get; set; } = new();
}