using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Constants;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.Data.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
                builder.ToTable("permissions");

                builder.HasKey(p => p.Id)
                        .HasName("pk_permissions");

                builder.Property(p => p.Id)
                        .HasColumnName("id")
                        .ValueGeneratedNever();

                builder.Property(p => p.Name)
                        .HasColumnName("name")
                        .IsRequired()
                        .HasMaxLength(100);
                
                builder.HasMany(p => p.RolePermissions)
                        .WithOne(rp => rp.Permission)
                        .HasForeignKey(rp => rp.PermissionId)
                        .OnDelete(DeleteBehavior.Cascade);

                builder.HasData(Enum.GetValues<PermissionEnum>()
                        .Select(p => new Permission
                        {
                                Id = SystemPermissionSettings.GetPermissionGuid(p),
                                Name = p.ToString()
                        }));
        }
}