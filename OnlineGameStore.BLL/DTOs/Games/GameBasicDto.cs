using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs.Games;

public class GameBasicDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }
}