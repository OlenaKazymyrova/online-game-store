using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Enums;
using OnlineGameStore.SharedLogic.Settings;

namespace OnlineGameStore.DAL.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(r => r.Id)
            .HasName("pk_roles");

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(r => r.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.HasIndex(r => r.Name)
            .HasDatabaseName("ix_roles_name")
            .IsUnique();

        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(Enum.GetValues<RoleEnum>()
            .Select(r => new Role
            {
                Id = SystemPermissionSettings.GetRoleGuid(r),
                Name = r.ToString(),
                Description = SystemPermissionSettings.GetRoleDescription(r)
            }));
    }
}