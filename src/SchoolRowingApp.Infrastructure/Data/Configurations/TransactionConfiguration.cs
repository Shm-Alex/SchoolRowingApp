// Infrastructure/Data/Configurations/TransactionConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolRowingApp.Domain.Banking;

namespace SchoolRowingApp.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions", "banking");

        // Составной первичный ключ: OperationDate + Amount + Currency
        builder.HasKey(t => new { t.OperationDate, t.Amount, t.Currency });

        // Остальные индексы для производительности
        builder.HasIndex(t => t.PaymentDate);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Category);
        builder.HasIndex(t => t.IsProcessed);

        // Настройка свойств
        builder.Property(t => t.OperationDate)
               .IsRequired();

        builder.Property(t => t.PaymentDate)
               .IsRequired();

        builder.Property(t => t.CardLastDigits)
               .HasMaxLength(4);

        builder.Property(t => t.Status)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(t => t.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(t => t.Currency)
               .IsRequired()
               .HasMaxLength(3);

        builder.Property(t => t.PaymentAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(t => t.PaymentCurrency)
               .IsRequired()
               .HasMaxLength(3);

        builder.Property(t => t.Cashback)
               .HasColumnType("decimal(18,2)");

        builder.Property(t => t.Category)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(t => t.MccCode)
               .HasMaxLength(4);

        builder.Property(t => t.Description)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(t => t.BonusAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(t => t.RoundUpAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(t => t.OperationAmountWithRoundUp)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(t => t.IsProcessed)
               .IsRequired();
    }
}