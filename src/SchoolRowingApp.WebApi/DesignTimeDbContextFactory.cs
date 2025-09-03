// SchoolRowingApp/src/SchoolRowingApp.WebApi/DesignTimeDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SchoolRowingApp.Infrastructure.Data;

namespace SchoolRowingApp.WebApi;
/*
 что за???

 */
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=rowingclub;Username=rowing_user;Password=StrongPass!2024;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}