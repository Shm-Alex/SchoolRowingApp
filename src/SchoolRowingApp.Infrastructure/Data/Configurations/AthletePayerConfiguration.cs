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

        // Составной первичный ключ
        builder.HasKey(ap => new { ap.AthleteId, ap.PayerId });

        // Явно указываем, что AthleteId используется как внешний ключ
        builder.HasOne(ap => ap.Athlete)
               .WithMany(a => a.AthletePayers)
               .HasForeignKey(ap => ap.AthleteId)
               .HasPrincipalKey(a => a.Id)
               .OnDelete(DeleteBehavior.Cascade);

        // Явно указываем, что PayerId используется как внешний ключ
        builder.HasOne(ap => ap.Payer)
               .WithMany(p => p.AthletePayers)
               .HasForeignKey(ap => ap.PayerId)
               .HasPrincipalKey(p => p.Id)
               .OnDelete(DeleteBehavior.Cascade);

        // Настройка PayerType
        builder.Property(ap => ap.PayerType)
               .HasConversion<int>();
    }
}