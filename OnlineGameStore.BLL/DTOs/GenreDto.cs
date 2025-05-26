namespace OnlineGameStore.BLL.DTOs;

public class GenreDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description {  get; set; }
    public Guid? ParentId { get; set; }
}
