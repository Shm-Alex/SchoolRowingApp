// Infrastructure/Data/Configurations/AthleteMembershipConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Membership;

namespace SchoolRowingApp.Infrastructure.Data.Configurations;

/// <summary>
/// Конфигурация для сущности AthleteMembership.
/// Определяет структуру таблицы AthleteMemberships в базе данных.
/// </summary>
public class AthleteMembershipConfiguration : IEntityTypeConfiguration<AthleteMembership>
{
    public void Configure(EntityTypeBuilder<AthleteMembership> builder)
    {
        builder.ToTable("AthleteMemberships", "bob");

        // Составной первичный ключ: AthleteId + MembershipPeriod
        builder.HasKey(am => new { am.AthleteId, am.MembershipPeriodYear, am.MembershipPeriodMonth });

        // Составной внешний ключ на MembershipPeriod
        builder.HasOne(am => am.MembershipPeriod)
               .WithMany(mp => mp.AthleteMemberships)
               .HasForeignKey(am => new { am.MembershipPeriodYear, am.MembershipPeriodMonth })
               .HasPrincipalKey(mp => new { mp.Year, mp.Month })
               .OnDelete(DeleteBehavior.Cascade);

        // Отношение к Athlete (зависимая сторона)
        builder.HasOne(am => am.Athlete)
               .WithMany(a => a.AthleteMemberships)
               .HasForeignKey(am => am.AthleteId)
               .HasPrincipalKey(a => a.Id)
               .OnDelete(DeleteBehavior.Cascade);

        // Ограничение на коэффициент участия
        builder.Property(am => am.ParticipationCoefficient)
               .HasColumnType("decimal(3,1)")
               .IsRequired();
    }
}
