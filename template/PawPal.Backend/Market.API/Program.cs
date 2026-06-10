using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using PawPal.API;
using PawPal.API.Middleware;
using PawPal.Application;
using PawPal.Application.Abstractions;
using PawPal.Application.Services;
using PawPal.Infrastructure;
using PawPal.Infrastructure.Common;
using PawPal.Infrastructure.Signal;
using Serilog;
using System.Security.Cryptography;

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
                    // Cleaned up to use your configured allowedOrigins as well
                    policy.WithOrigins("http://localhost:4200", "https://localhost:7260", allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Required for SignalR
                });
            });

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            builder.Services.AddTransient<IEmailService, EmailService>();

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("firebase-service-account.json")
            });

            builder.Services.AddSingleton<FirebaseNotificationService>();

            var app = builder.Build();

            // ---------------------------------------------------------
            // 4. Middleware pipeline
            // ---------------------------------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors();

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