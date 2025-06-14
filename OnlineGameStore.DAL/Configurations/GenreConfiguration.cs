using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Constants;

namespace OnlineGameStore.DAL.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(genre => genre.Id);

        builder.Property(genre => genre.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(genre => genre.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(GenreConstants.NameMaxLength);

        builder.Property(genre => genre.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasMaxLength(GenreConstants.DescriptionMaxLength);

        builder.HasOne(genre => genre.ParentGenre)
            .WithMany()
            .HasForeignKey(genre => genre.ParentId)
            .OnDelete(DeleteBehavior.SetNull);

        //builder.HasMany(genre => genre.Games)
        //    .WithMany(game => game.Genres)
        //    .UsingEntity(j => j.ToTable("GameGenres;

        builder
            .HasMany(genre => genre.Games)
            .WithMany(game => game.Genres)
            .UsingEntity<Dictionary<string, object>>(
                "GameGenres",
                j => j
                    .HasOne<Game>()
                    .WithMany()
                    .HasForeignKey("game_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("genre_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("GameGenres");
                    j.HasKey("game_id", "genre_id");

                    j.Property<Guid>("game_id")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    j.Property<Guid>("genre_id")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");
                });


    }
}