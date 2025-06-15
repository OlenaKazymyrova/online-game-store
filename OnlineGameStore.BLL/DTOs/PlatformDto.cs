using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs;

public class PlatformDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public ICollection<Guid> GamesIds { get; set; } = new List<Guid>();
}