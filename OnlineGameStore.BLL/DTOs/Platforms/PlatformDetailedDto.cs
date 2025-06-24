using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using OnlineGameStore.BLL.DTOs.Games;

namespace OnlineGameStore.BLL.DTOs.Platforms;

public class PlatformDetailedDto
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<GameBasicDto>? Games { get; set; } = new List<GameBasicDto>();
}