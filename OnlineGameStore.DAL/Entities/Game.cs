using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL.Entities;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(256)]
    public required string Name { get; set; } = string.Empty;
    [MaxLength(4096)]
    public required string Description { get; set; } = string.Empty;
    public required Guid Publisher { get; set; }
    public required Guid Genre { get; set; }
    public required Guid License { get; set; }
}