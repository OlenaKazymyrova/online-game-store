using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL.Entities;

public class UserRole
{
    [Required]
    public required Guid UserId { get; set; }
    public User User { get; set; }
    [Required]
    public required Guid RoleId { get; set; }
    public Role Role { get; set; }
}