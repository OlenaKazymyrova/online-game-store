using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL.Entities;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(256)]
    public required string Name { get; set; } = string.Empty;
    [MaxLength(4096)]
    public required string Description { get; set; } = string.Empty;
    public Guid? Publisher { get; set; }
    public Guid? Genre { get; set; }
    public Guid? License { get; set; }
}