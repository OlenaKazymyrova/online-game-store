using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs;

public class GameDto
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? PublisherId { get; set; }
    public Guid? GenreId { get; set; }
    public Guid? LicenseId { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
    public decimal Price { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }
}