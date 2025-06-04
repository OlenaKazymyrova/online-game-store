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

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(g => g.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(GenreConstants.NameMaxLength);

        builder.Property(g => g.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasMaxLength(GenreConstants.DescriptionMaxLength);

        builder.HasOne(g => g.ParentGenre)
            .WithMany()
            .HasForeignKey(g => g.ParentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}