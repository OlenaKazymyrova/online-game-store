using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL;

public class Game
{
    public long Id { get; set; }
    [MaxLength(256)]
    public required string Name { get; set; } = string.Empty;
    [MaxLength(4096)]
    public required string Description { get; set; } = string.Empty;
    public required long Publisher { get; set; }
    public required long Genre { get; set; }
    public required long License { get; set; }
}