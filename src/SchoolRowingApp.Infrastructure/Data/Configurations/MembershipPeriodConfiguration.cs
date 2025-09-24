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
        builder.ToTable("MembershipPeriods", "bob", table =>
        {
            table.HasCheckConstraint("CK_Month", "Month >= 1 AND Month <= 12");
            table.HasCheckConstraint("CK_Year", "Year >= 2020 AND Year <= 2100");
            table.HasCheckConstraint("CK_BaseFee", "BaseFee >= 0");
        });

        // Устанавливаем составной первичный ключ
        builder.HasKey(mp => new { mp.Year, mp.Month });

        // Остальные настройки свойств
        builder.Property(mp => mp.Month)
               .IsRequired();

        builder.Property(mp => mp.Year)
               .IsRequired();

        builder.Property(mp => mp.BaseFee)
               .IsRequired()
               .HasColumnType("decimal(10,2)");

        // Настройка отношения "один ко многим" с AthleteMembership
        builder
            .HasMany(mp => mp.AthleteMemberships)
            .WithOne(am => am.MembershipPeriod)
            .HasForeignKey(am => new { am.MembershipPeriodYear, am.MembershipPeriodMonth })
            .HasPrincipalKey(mp => new { mp.Year, mp.Month })
            .OnDelete(DeleteBehavior.Cascade);
    }
}