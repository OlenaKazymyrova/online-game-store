namespace OnlineGameStore.DAL.Entities;

public class User : TEntity
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}