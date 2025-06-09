namespace OnlineGameStore.DAL.Entities;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public Guid? PublisherId { get; set; }
    public Guid? GenreId { get; set; }
    public Guid? LicenseId { get; set; }
    public required decimal Price { get; set; }
    public required DateTime ReleaseDate { get; set; }
    public ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}