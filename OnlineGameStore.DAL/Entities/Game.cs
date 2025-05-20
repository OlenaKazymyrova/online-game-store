using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL.Entities;

public class Game
{
    public int Id { get; set; }
    [MaxLength(256)]
    public required string Name { get; set; } = string.Empty;
    [MaxLength(4096)]
    public required string Description { get; set; } = string.Empty;
    public required int Publisher { get; set; }
    public required int Genre { get; set; }
    public required int License { get; set; }
}