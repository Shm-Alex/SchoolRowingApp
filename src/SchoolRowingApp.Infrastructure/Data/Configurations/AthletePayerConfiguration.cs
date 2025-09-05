using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Payments;

namespace SchoolRowingApp.Infrastructure.Data.Configurations;

public class AthletePayerConfiguration : IEntityTypeConfiguration<AthletePayer>
{
    public void Configure(EntityTypeBuilder<AthletePayer> builder)
    {
        builder.ToTable("AthletePayers");

        builder.HasKey(ap => new { ap.AthleteId, ap.PayerId });
        
        builder.Property(ap => ap.AthleteId)
               .HasColumnName("AthleteId");

        builder.Property(ap => ap.PayerId)
               .HasColumnName("PayerId");

        builder.HasOne<Payer>()
               .WithMany()
               .HasForeignKey(ap => ap.PayerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Athlete>()
               .WithMany(a => a.AthletePayers)
               .HasForeignKey(ap => ap.AthleteId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ap => ap.PayerType)
               .HasConversion<int>();
    }
}