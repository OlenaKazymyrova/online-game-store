
namespace OnlineGameStore.DAL.Entities;

public class Genre
{
    public required Guid Id { get; set; } = Guid.Empty;

    public required string Name { get; set; } = string.Empty;

    public required string Description { get; set; } = string.Empty;

    public Genre? ParentGenre { get; set; } = default;

    public ICollection<Game> Games { get; set; } = new List<Game>();

}
