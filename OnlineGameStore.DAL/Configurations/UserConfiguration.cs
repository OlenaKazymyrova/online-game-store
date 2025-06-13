using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .IsRequired();
        
        builder.HasKey(g => g.Id);

        builder.Property(g => g.UserName)
            .HasColumnName("username")
            .IsRequired();
        
        builder.HasIndex(u => u.UserName)
            .IsUnique()
            .HasDatabaseName("IX_Users_UserName");
        
        builder.Property(g => g.Email)
            .HasColumnName("email")
            .IsRequired();
        
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        builder.Property(g => g.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();
        
        builder.Property(g => g.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(g => g.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NULL");
    }
}