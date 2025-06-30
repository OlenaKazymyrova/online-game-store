using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL.Entities;

public class Role : TEntity
{
    [Required] 
    public override Guid Id { get; set; } = Guid.NewGuid();
    [Required] 
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public ICollection<User> Users = new List<User>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}