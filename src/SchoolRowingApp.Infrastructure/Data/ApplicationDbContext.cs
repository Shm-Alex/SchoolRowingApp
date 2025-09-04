using Microsoft.EntityFrameworkCore;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Athlete> Athletes { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}