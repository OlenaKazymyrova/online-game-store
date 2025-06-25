using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using OnlineGameStore.BLL.DTOs.Games;

namespace OnlineGameStore.BLL.DTOs.Genres;
public class GenreDetailedDto
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    public Guid? ParentId { get; set; } = default;

    [Required]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<GameBasicDto>? Games { get; set; } = new List<GameBasicDto>();
}