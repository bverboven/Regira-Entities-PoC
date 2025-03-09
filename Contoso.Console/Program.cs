using Contoso.Console;
using Contoso.Constants;
using Contoso.Data;
using Contoso.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// prevent date errors in Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = new HostBuilder()
    .ConfigureAppConfiguration((_, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        config.AddUserSecrets<Program>();
        config.AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        services.AddContosoEntities(dbOptionsBuilder =>
            {
                Console.WriteLine($"Installing at {AppSettings.DataFolder}");
                Directory.CreateDirectory(AppSettings.DataFolder);
                dbOptionsBuilder
                    .UseSqlite(AppSettings.ConnectionString, sqliteBuilder => sqliteBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            })
        .AddTransient<DataSeeder>();
    });

var host = builder.Build();

var sp = host.Services;
var db = sp.GetRequiredService<ContosoContext>();

await db.Database.EnsureDeletedAsync();
if (await db.Database.EnsureCreatedAsync())
{
    Console.WriteLine("Database created");
}
var seeder = sp.GetRequiredService<DataSeeder>();
await seeder.SeedDataAsync(1);

Console.WriteLine("The End");

await host.RunAsync();