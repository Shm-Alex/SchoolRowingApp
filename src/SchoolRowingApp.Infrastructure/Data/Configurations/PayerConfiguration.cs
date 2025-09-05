using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Payments;

namespace SchoolRowingApp.Infrastructure.Data.Configurations;

public class PayerConfiguration : IEntityTypeConfiguration<Payer>
{
    public void Configure(EntityTypeBuilder<Payer> builder)
    {
        builder.ToTable("Payers");

        builder.HasKey(p => p.Id);
        // Явно указываем отношение к AthletePayer
        builder.HasMany(p => p.AthletePayers)
               .WithOne(ap => ap.Payer)
               .HasForeignKey(ap => ap.PayerId);

     

        builder.Property(p => p.FirstName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.SecondName)
               .HasMaxLength(50);

        builder.Property(p => p.LastName)
               .IsRequired()
               .HasMaxLength(50);
    }
}