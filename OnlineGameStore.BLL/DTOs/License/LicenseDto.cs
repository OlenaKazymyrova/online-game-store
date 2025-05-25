using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs;

public class LicenseDto
{
    [Required] 
    [MaxLength(500)] 
    public string Description { get; set; }

    [Range(0, double.MaxValue)] 
    public decimal Cost { get; set; }
}
