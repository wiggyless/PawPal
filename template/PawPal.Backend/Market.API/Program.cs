using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using PawPal.API;
using PawPal.API.Middleware;
using PawPal.Application;
using PawPal.Application.Abstractions;
using PawPal.Application.Options;
using PawPal.Application.Services;
using PawPal.Infrastructure;
using PawPal.Infrastructure.BackgroundServices;
using PawPal.Infrastructure.Common;
using PawPal.Infrastructure.Signal;
using Serilog;

using System.Security.Cryptography;
using System.Threading.RateLimiting;
public partial class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting PawPal API...");

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, services, cfg) =>
            {
                cfg.ReadFrom.Configuration(ctx.Configuration)
                   .ReadFrom.Services(services)
                   .Enrich.FromLogContext()
                   .Enrich.WithThreadId()
                   .Enrich.WithProcessId()
                   .Enrich.WithMachineName();
            });

            builder.Logging.ClearProviders();

            // ---------------------------------------------------------
            // 3. Layer registrations
            // ---------------------------------------------------------
            builder.Services
                .AddAPI(builder.Configuration, builder.Environment)
                .AddInfrastructure(builder.Configuration, builder.Environment)
                .AddApplication()
                .AddSignalR();
            builder.Services.Configure<AppUrlsOptions>(
                    builder.Configuration.GetSection(AppUrlsOptions.SectionName));

            builder.Services.AddScoped<ICommentHubService, CommentHubService>();
            builder.Services.AddScoped<IMessageHubService, MessageHubService>();
            builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

            builder.Services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
             
                var existingOnMessageReceived = options.Events?.OnMessageReceived;

                options.Events ??= new JwtBearerEvents();
                options.Events.OnMessageReceived = async context =>
                {
                    if (existingOnMessageReceived != null)
                    {
                        await existingOnMessageReceived(context);
                    }

                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/commentHub") || path.StartsWithSegments("/messageHub")))
                    {
                        context.Token = accessToken;
                    }
                };
            });
            
            var allowedOrigins = builder.Configuration.GetValue<string>("allowedOrigins") ?? "http://localhost:4200";
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://localhost:4200", allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                           .WithExposedHeaders("Content-Disposition");
                });
            });



            builder.Services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        message = "Too many requests. Please slow down.",
                        retryAfterSeconds = 60
                    }, token);
                };

                options.AddPolicy("IpBasedPolicy", httpContext =>
                {
                    var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                    return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,   
                        Window = TimeSpan.FromSeconds(60),
                        QueueLimit = 0
                    });
                });
            });

            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddHostedService<ExpiredTokenCleanupService>();

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("firebase-service-account.json")
            });
            builder.Services.AddSingleton<IFirebaseNotificationService, FirebaseNotificationService>();

            var app = builder.Build();

            // ---------------------------------------------------------
            // 4. Middleware pipeline
            // ---------------------------------------------------------
            app.UseMiddleware<InputSanitizationMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRateLimiter();
            app.UseExceptionHandler();

            app.UseCors();

            app.Use(async (context, next) =>
            {
                var path = context.Request.Path.Value ?? "";
                var isImage = path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                              path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                              path.EndsWith(".webp", StringComparison.OrdinalIgnoreCase);

                if (isImage)
                {
                    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                }
                await next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:4200";
                    ctx.Context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<CommentHub>("/commentHub");
            app.MapHub<MessageHub>("/messageHub");
            app.MapControllers();

            await app.Services.InitializeDatabaseAsync(app.Environment);

            Log.Information("Pawpal API started successfully.");
            app.Run();
        }
        catch (HostAbortedException)
        {
            Log.Information("Host aborted by EF Core tooling (design-time) - its ok.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Pawpal API terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}