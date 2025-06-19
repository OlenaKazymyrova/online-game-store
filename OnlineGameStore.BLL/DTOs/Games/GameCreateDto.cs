using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs.Games;

public class GameCreateDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? PublisherId { get; set; }
    public Guid? LicenseId { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
    public decimal Price { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }
    [Required]
    public ICollection<Guid> GenresIds { get; set; } = new List<Guid>();
    [Required]
    public ICollection<Guid> PlatformsIds { get; set; } = new List<Guid>();
}
