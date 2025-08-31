using SchoolRowingApp.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();  // ← вместо MapGroup

app.Run();

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

//builder.Services.AddApplicationServices();
//builder.Services.AddInfrastructureServices(builder.Configuration);
//builder.Services.AddWebServices();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    await app.InitialiseDatabaseAsync();
//}
//else
//{
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHealthChecks("/health");
//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseSwaggerUi(settings =>
//{
//    settings.Path = "/api";
//    settings.DocumentPath = "/api/specification.json";
//});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action=Index}/{id?}");

//app.MapRazorPages();

//app.MapFallbackToFile("index.html");

//app.UseExceptionHandler(options => { });

//#if (UseApiOnly)
//app.Map("/", () => Results.Redirect("/api"));
//#endif

//app.MapEndpoints();

//app.Run();

//public partial class Program { }
