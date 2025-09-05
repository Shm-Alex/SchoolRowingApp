using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolRowingApp.Domain.Athletes;

namespace SchoolRowingApp.Infrastructure.Data.Configurations;

public class AthleteConfiguration : IEntityTypeConfiguration<Athlete>
{
    public void Configure(EntityTypeBuilder<Athlete> builder)
    {
        builder.ToTable("Athletes");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.FirstName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(a => a.SecondName)
               .HasMaxLength(50);

        builder.Property(a => a.LastName)
               .IsRequired()
               .HasMaxLength(50);
        // Явно указываем отношение к AthletePayer
        builder.HasMany(a => a.AthletePayers)
               .WithOne(ap => ap.Athlete)
               .HasForeignKey(ap => ap.AthleteId);
    }
}