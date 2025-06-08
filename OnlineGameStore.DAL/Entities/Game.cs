namespace OnlineGameStore.DAL.Entities;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public Guid? Publisher { get; set; }
    public Guid? Genre { get; set; }
    public Guid? License { get; set; }
    public required decimal Price { get; set; }
    public required DateTime ReleaseDate { get; set; }
    public ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}