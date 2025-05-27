using System.ComponentModel.DataAnnotations;
using OnlineGameStore.DAL.Constants;

namespace OnlineGameStore.DAL.Entities;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(GameConstants.NameMaxLength)]
    public required string Name { get; set; } = string.Empty;
    [MaxLength(GameConstants.DescriptionMaxLength)]
    public required string Description { get; set; } = string.Empty;
    public Guid? Publisher { get; set; }
    public Guid? Genre { get; set; }
    public License? License { get; set; } // change just to on-to-one relation
}