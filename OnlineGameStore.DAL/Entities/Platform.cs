using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL.Entities;

public class Platform
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public ICollection<GamePlatform> GamePlatforms { get; set; }
}