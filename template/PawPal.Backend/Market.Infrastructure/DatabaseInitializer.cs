using Market.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PawPal.Infrastructure.Database.Seeders;
using PawPal.Shared.Constants;

namespace PawPal.Infrastructure;

public static class DatabaseInitializer
{
    /// <summary>
    /// Centralized migration and seeding.
    /// </summary>
    public static async Task InitializeDatabaseAsync(this IServiceProvider services, IHostEnvironment env)
    {
        await using var scope = services.CreateAsyncScope();
        var ctx = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        if (env.IsTest())
        {
            await ctx.Database.EnsureCreatedAsync();
            await DynamicDataSeeder.SeedAsync(ctx);
            return;
        }

        // SQL Server or similar
        await ctx.Database.MigrateAsync();

        if (env.IsDevelopment())
        {
            await DynamicDataSeeder.SeedAsync(ctx);
        }
    }
}