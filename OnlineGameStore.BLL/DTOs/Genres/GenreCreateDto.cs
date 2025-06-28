using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs.Genres;

public class GenreCreateDto
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    public Guid? ParentId { get; set; } = null;
}
