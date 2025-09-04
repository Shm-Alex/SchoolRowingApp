using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRowingApp.Infrastructure.Data.Configurations
{
    // Infrastructure/Data/Configurations/AthletePayerConfiguration.cs
            //public class AthletePayerConfiguration : IEntityTypeConfiguration<AthletePayer>
            //{
            //    public void Configure(EntityTypeBuilder<AthletePayer> builder)
            //    {
            //        builder.ToTable("AthletePayers");

            //        builder.HasKey(ap => new { ap.AthleteId, ap.PayerId, ap.PayerType });

            //        builder.HasOne<Payer>()
            //               .WithMany()
            //               .HasForeignKey(ap => ap.PayerId)
            //               .OnDelete(DeleteBehavior.Cascade);

            //        builder.HasOne<Athlete>()
            //               .WithMany(a => a.AthletePayers)
            //               .HasForeignKey(ap => ap.AthleteId)
            //               .OnDelete(DeleteBehavior.Cascade);

            //        // Храним enum как integer (по умолчанию)
            //        builder.Property(ap => ap.PayerType)
            //               .HasConversion<int>();
            //    }
            //}
}
