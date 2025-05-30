namespace OnlineGameStore.BLL.DTOs;

public class GameDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? PublisherId { get; set; }
    public Guid? GenreId { get; set; }
    public Guid? LicenseId { get; set; }
    public required decimal Price { get; set; }
    public required DateTime ReleaseDate { get; set; }
}