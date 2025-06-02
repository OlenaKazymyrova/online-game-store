using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations;

public class GamePlatformConfiguraiton : IEntityTypeConfiguration<GamePlatform>
{
    public void Configure(EntityTypeBuilder<GamePlatform> builder)
    {
        builder.ToTable("Game_Platform");
        builder.HasKey(gp => new { gp.GameId, gp.PlatformId });
        builder.HasOne(gp => gp.Game)
            .WithMany(g => g.GamePlatforms)
            .HasForeignKey(gp => gp.GameId).
            OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gp => gp.Platform)
            .WithMany(p => p.GamePlatforms)
            .HasForeignKey(gp => gp.PlatformId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}