using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.BLL.DTOs.Platforms;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OnlineGameStore.BLL.DTOs.Games;

public class GameDetailedDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public Guid? PublisherId { get; set; }

    public Guid? LicenseId { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
    public decimal Price { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<GenreBasicDto>? Genres { get; set; } = new List<GenreBasicDto>();

    [Required]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<PlatformBasicDto>? Platforms { get; set; } = new List<PlatformBasicDto>();
}