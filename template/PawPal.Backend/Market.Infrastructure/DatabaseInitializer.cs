using Market.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Hosting;
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
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var envr = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        if (env.IsTest())
        {
            await ctx.Database.EnsureCreatedAsync();
            await DynamicDataSeeder.SeedAsync(ctx,sender, envr);
            return;
        }

        // SQL Server or similar
        await ctx.Database.MigrateAsync();

        if (env.IsDevelopment())
        {
            await DynamicDataSeeder.SeedAsync(ctx,sender, envr);
        }
    }
}