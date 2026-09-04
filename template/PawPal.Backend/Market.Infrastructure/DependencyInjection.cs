using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Market.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PawPal.Application.Abstractions;
using PawPal.Domain.Entities.Identity;
using PawPal.Infrastructure.Common;
using PawPal.Shared.Constants;
using PawPal.Shared.Options;
using Serilog;

namespace PawPal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env)
    {
        // Typed ConnectionStrings + validation
        services.AddOptions<ConnectionStringsOptions>()
            .Bind(configuration.GetSection(ConnectionStringsOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // DbContext: InMemory for test environments; SQL Server otherwise
        services.AddDbContext<DatabaseContext>((sp, options) =>
        {
            if (env.IsTest())
            {
                options.UseInMemoryDatabase("IntegrationTestsDb");

                return;
            }

            var cs = sp.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value.Main;
            options.UseSqlServer(cs);
        });

        // IAppDbContext mapping
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<DatabaseContext>());

        // Identity hasher
        services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

        // Token service (reads JwtOptions via IOptions<JwtOptions>)
        services.AddTransient<IJwtTokenService, JwtTokenService>();

        // HttpContext accessor + current user
        services.AddHttpContextAccessor();
        services.AddScoped<IAppCurrentUser, AppCurrentUser>();

        // Upload handling (post images, user avatars, news photos)
        services.AddScoped<IFileStorageService, FileStorageService>();

        // Firebase push notifications — absence of the service account file just disables
        // sending (logged as a warning, not fatal).
        var firebaseCredentialsPath = Path.Combine(env.ContentRootPath, "firebase-service-account.json");
        if (File.Exists(firebaseCredentialsPath))
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(firebaseCredentialsPath)
            });
        }
        else
        {
            Log.Warning("firebase-service-account.json not found at {Path}. Push notifications will be disabled.", firebaseCredentialsPath);
        }
        services.AddSingleton<IFirebaseNotificationService, FirebaseNotificationService>();

        // TimeProvider (if used in handlers/services)
        services.AddSingleton(TimeProvider.System);

        return services;
    }
}