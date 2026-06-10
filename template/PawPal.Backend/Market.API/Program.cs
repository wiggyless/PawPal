﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using PawPal.API;
using PawPal.API.Middleware;
using PawPal.Application;
using PawPal.Application.Abstractions;
using PawPal.Infrastructure;
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
            builder.Services.AddScoped<ICommentHubService, CommentHubService>();

            // FIX: Instead of calling .AddAuthentication().AddJwtBearer() again,
            // we safely intercept the existing "Bearer" options setup.
            builder.Services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                // Save any existing OnMessageReceived logic if something else was using it
                var existingOnMessageReceived = options.Events?.OnMessageReceived;

                options.Events ??= new JwtBearerEvents();
                options.Events.OnMessageReceived = async context =>
                {
                    // 1. Run the original token logic first if there was any
                    if (existingOnMessageReceived != null)
                    {
                        await existingOnMessageReceived(context);
                    }

                    // 2. Fall back to extracting the token from query string for SignalR
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/commentHub"))
                    {
                        context.Token = accessToken;
                    }
                };
            });
            // CORS Policy setup
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

            // 1. Add Rate Limiting Services
            builder.Services.AddRateLimiter(options =>
            {
                // Define what happens when a request is rejected
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

                // Example 1: A Named Policy (Fixed Window based on IP Address)
                options.AddPolicy("IpBasedPolicy", httpContext =>
                {
                    // Get client IP address or default to "anonymous"
                    var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                    return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,           // Max 10 requests
                        Window = TimeSpan.FromSeconds(60), // Per 1 minute
                        QueueLimit = 0              // Reject instantly if limit is exceeded
                    });
                });
            });


            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            builder.Services.AddTransient<IEmailService, EmailService>();


            var app = builder.Build();

            // ---------------------------------------------------------
            // 4. Middleware pipeline
            // ---------------------------------------------------------
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

            app.UseHttpsRedirection(); // ← now after your middleware
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<CommentHub>("/commentHub");
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