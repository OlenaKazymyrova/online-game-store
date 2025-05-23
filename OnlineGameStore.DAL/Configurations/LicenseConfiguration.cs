using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations;

public class LicenseConfiguration : IEntityTypeConfiguration<License>
{
    public void Configure(EntityTypeBuilder<License> builder)
    {
        builder.ToTable("Licenses");
        
        builder.HasKey(l => l.Id);

        builder.Property(l => l.GameId)
            .IsRequired(); // add check that such id exist

        builder.Property(l => l.Description)
            .HasMaxLength(500)
            .IsUnicode(true);

        builder.Property(l => l.Cost)
            .HasColumnType("decimal(10, 2)")
            .HasDefaultValue(0m);

        builder.HasCheckConstraint("CK_License_Cost", "Cost>=0");
        
        builder.HasOne(l => l.Game).
            WithMany(g => g.Licenses)
            .HasForeignKey(l => l.GameId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}