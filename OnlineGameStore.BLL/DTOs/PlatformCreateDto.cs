using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs;

public class PlatformCreateDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(1, ErrorMessage = "Name cannot be empty.")]
    [RegularExpression(@"\S.*", ErrorMessage = "Name cannot be just whitespace.")]
    public string Name { get; set; } = string.Empty;
}