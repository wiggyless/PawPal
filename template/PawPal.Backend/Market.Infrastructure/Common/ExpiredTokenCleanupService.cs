using Market.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PawPal.Infrastructure.BackgroundServices;

public sealed class ExpiredTokenCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ExpiredTokenCleanupService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public ExpiredTokenCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<ExpiredTokenCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ExpiredTokenCleanupService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanUpExpiredTokensAsync(stoppingToken);
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CleanUpExpiredTokensAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var now = DateTime.UtcNow;

            var expiredUsers = await db.Users
                .Where(u =>
                    u.IsEmailConfirmed == false &&
                    u.EmailConfirmationToken != null &&
                    u.EmailConfirmationTokenExpiresAt != null &&
                    u.EmailConfirmationTokenExpiresAt < now)
                .ToListAsync(cancellationToken);

            if (expiredUsers.Count == 0)
            {
                _logger.LogInformation("No expired tokens found.");
                return;
            }

            foreach (var user in expiredUsers)
            {
                user.EmailConfirmationToken = null;
                user.EmailConfirmationTokenExpiresAt = null;
            }

            await db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Cleaned up {Count} expired confirmation token(s) at {Time}.",
                expiredUsers.Count, now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during expired token cleanup.");
        }
    }
}