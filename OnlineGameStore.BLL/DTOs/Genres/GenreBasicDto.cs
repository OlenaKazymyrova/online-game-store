using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs.Genres;

public class GenreBasicDto
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }
}
