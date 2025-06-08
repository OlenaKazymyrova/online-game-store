using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Constants;

namespace OnlineGameStore.DAL.Configurations;

public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.ToTable("Platform");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(PlatformConstants.NameMaxLength);

        builder.HasMany(p => p.Games)
            .WithMany(g => g.Platforms)
            .UsingEntity(j => j.ToTable("GamePlatforms"));
    }
}