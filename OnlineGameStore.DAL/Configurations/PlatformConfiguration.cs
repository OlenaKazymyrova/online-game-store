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
        
        builder
            .HasMany(p => p.Games)
            .WithMany(g => g.Platforms)
            .UsingEntity<Dictionary<string, object>>(
                "GamePlatforms",
                j => j
                    .HasOne<Game>()
                    .WithMany()
                    .HasForeignKey("game_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Platform>()
                    .WithMany()
                    .HasForeignKey("platform_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("GamePlatforms");
                    j.HasKey("game_id", "platform_id");

                    j.Property<Guid>("game_id")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    j.Property<Guid>("platform_id")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");
                });
    }
}