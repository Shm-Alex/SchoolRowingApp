// Infrastructure/Data/Configurations/AthleteMembershipConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        // Составной первичный ключ: AthleteId + MembershipPeriodId
        builder.HasKey(am => new { am.AthleteId, am.MembershipPeriodId });

        builder.Property(am => am.ParticipationCoefficient)
               .HasColumnType("decimal(3,1)")
               .IsRequired();

        // Настройка отношения к Athlete
       builder.HasOne(am => am.Athlete)
               .WithMany(mp=>mp.AthleteMemberships) 
               .HasForeignKey(am => am.AthleteId)
               .HasPrincipalKey(a => a.Id)
               .OnDelete(DeleteBehavior.Cascade);


        // Настройка отношения к MembershipPeriod
        builder.HasOne(am => am.MembershipPeriod)
               .WithMany(mp => mp.AthleteMemberships)
               .HasForeignKey(am => am.MembershipPeriodId)
               .HasPrincipalKey(mp => mp.Id)
               .OnDelete(DeleteBehavior.Cascade);
    }
}