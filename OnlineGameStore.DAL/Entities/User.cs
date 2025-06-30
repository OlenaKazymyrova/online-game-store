using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL.Entities;

public class User : TEntity
{
    [Required]
    public override Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}