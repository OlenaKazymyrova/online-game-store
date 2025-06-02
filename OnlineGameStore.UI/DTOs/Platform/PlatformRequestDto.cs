using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.UI.DTOs.Platform;

public class PlatformRequestDto
{
    [Required(ErrorMessage = "Platform name is required.")]
    [StringLength(250, ErrorMessage = "Name cannot exceed 250 characters.")]
    public string Name { get; set; }

}