using Microsoft.EntityFrameworkCore;

using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Banking;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Domain.SharedKernel;


namespace SchoolRowingApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Athlete> Athletes { get; set; }
    public DbSet<Payer> Payers { get; set; }
    public DbSet<AthletePayer> AthletePayer { get; set; }
    public DbSet<MembershipPeriod> MembershipPeriods { get; set; }
    public DbSet<AthleteMembership> AthleteMemberships { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionImport> TransactionImports { get; set; }
    public DbSet<TransactionImportDetail> TransactionImportDetails { get; set; }



    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}