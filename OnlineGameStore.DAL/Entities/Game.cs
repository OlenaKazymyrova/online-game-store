using System.ComponentModel.DataAnnotations;
using OnlineGameStore.DAL.Constants;

namespace OnlineGameStore.DAL.Entities;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public Guid? Publisher { get; set; }
    public Guid? Genre { get; set; }
    public Guid? License { get; set; }
}