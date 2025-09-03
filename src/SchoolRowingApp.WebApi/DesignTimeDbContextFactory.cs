// SchoolRowingApp/src/SchoolRowingApp.WebApi/DesignTimeDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SchoolRowingApp.Infrastructure.Data;

namespace SchoolRowingApp.WebApi;
/*
 что за???
это добро нужно  для  создания  миграций 
PS C:\Users\alexandershm\source\repos\SchoolRowingApp\src> dotnet ef migrations add InitialCreate --project SchoolRowingApp.Infrastructure --startup-project SchoolRowingApp.WebApi
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
и для применения  миграций 
PS C:\Users\alexandershm\source\repos\SchoolRowingApp\src> dotnet ef database update --project SchoolRowingApp.Infrastructure --startup-project SchoolRowingApp.WebApi
Build started...
Build succeeded.
Applying migration '20250903154047_InitialCreate'.
Done.
PS C:\Users\alexandershm\source\repos\SchoolRowingApp\src>
 */
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Local.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}