using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL.Entities;

public class Role : Entity
{
    [Required]
    public override Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
}