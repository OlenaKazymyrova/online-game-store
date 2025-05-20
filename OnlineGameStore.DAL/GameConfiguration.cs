using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineGameStore.DAL;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("games");
        
        builder.HasKey(g => g.Id);
        
        builder.Property(g => g.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(g => g.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(g => g.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(4096);

        builder.Property(g => g.Publisher)
            .HasColumnName("publisher_id")
            .IsRequired();
        
        builder.Property(g => g.Genre)
            .HasColumnName("genre_id")
            .IsRequired();
        
        builder.Property(g => g.License)
            .HasColumnName("license_id")
            .IsRequired();
    }
}