namespace OnlineGameStore.BLL.DTOs.Roles;

public class RoleReadDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}