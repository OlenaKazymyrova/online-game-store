using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Constants;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations;

public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
{

    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.ToTable("Platforms");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(PlatformConstants.NameMaxLength);
        builder.HasIndex(p => p.Name).IsUnique();
    }
}