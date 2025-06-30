using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Settings;

namespace OnlineGameStore.DAL.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        builder.HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        builder.HasData(GetSeedData());
    }

    private static IEnumerable<RolePermission> GetSeedData()
    {
        foreach (var pair in SystemPermissionSettings.PermissionsByRole)
        {
            var roleId = SystemPermissionSettings.GetRoleGuid(pair.Key);
            foreach (var permission in pair.Value)
            {
                yield return new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = SystemPermissionSettings.GetPermissionGuid(permission)
                };
            }
        }
    }
}