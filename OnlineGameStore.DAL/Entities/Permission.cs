namespace OnlineGameStore.DAL.Entities;

public class Permission
{ 
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}