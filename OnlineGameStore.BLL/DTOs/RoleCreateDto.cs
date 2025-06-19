using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs;

public class RoleCreateDto
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Description { get; set; }
}