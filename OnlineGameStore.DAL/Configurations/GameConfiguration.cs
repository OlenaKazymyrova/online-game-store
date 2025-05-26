using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Constants;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("games");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(g => g.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(GameConstants.NameMaxLength);

        builder.Property(g => g.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(GameConstants.DescriptionMaxLength);

        builder.Property(g => g.Publisher)
            .HasColumnName("publisher_id");

        builder.Property(g => g.Genre)
            .HasColumnName("genre_id");


        //builder.HasOne(g => g.License)
        //   .WithOne(l => l.Game)
        //   .HasForeignKey<License>(l => l.GameId)
        //   .IsRequired();
    }
}