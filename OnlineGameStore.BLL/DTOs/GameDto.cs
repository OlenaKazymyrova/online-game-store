namespace OnlineGameStore.BLL.DTOs;

public class GameDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? Publisher { get; set; }
    public Guid? Genre { get; set; }
    public Guid? License { get; set; }
}