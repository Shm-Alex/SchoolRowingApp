// Infrastructure/Data/Configurations/MembershipPeriodConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolRowingApp.Domain.Membership;

namespace SchoolRowingApp.Infrastructure.Data.Configurations;

/// <summary>
/// Конфигурация для сущности MembershipPeriod.
/// Определяет структуру таблицы MembershipPeriods в базе данных.
/// </summary>
public class MembershipPeriodConfiguration : IEntityTypeConfiguration<MembershipPeriod>
{
    public void Configure(EntityTypeBuilder<MembershipPeriod> builder)
    {
        builder.ToTable("MembershipPeriods", "bob");

        builder.HasKey(mp => mp.Id);

        builder.Property(mp => mp.Month)
               .IsRequired();

        builder.Property(mp => mp.Year)
               .IsRequired();

        builder.Property(mp => mp.BaseFee)
               .HasColumnType("decimal(10,2)")
               .IsRequired();

        // Уникальный индекс для предотвращения дубликатов периодов
        builder.HasIndex(mp => new { mp.Year, mp.Month })
               .IsUnique();

        // Настройка отношения к AthleteMembership

        builder.Metadata.FindNavigation(nameof(MembershipPeriod.AthleteMemberships))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Или через HasMany с явным указанием поля
        builder.HasMany(typeof(AthleteMembership), "_athleteMembershipsCollection")
               .WithOne()
               .HasForeignKey("MembershipPeriodId")
               .OnDelete(DeleteBehavior.Cascade);

    }
}