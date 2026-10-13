using SnackisAPI.Seeding;

namespace SnackisAPI.Startup;

public static class DatabaseStartup
{
    public static async Task SeedDatabaseAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<DatabaseSeeder>().SeedAsync();
    }
}
