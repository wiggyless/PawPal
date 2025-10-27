﻿using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PawPal.Application.Common.Exceptions;
using PawPal.Shared.Dtos;
using System.Diagnostics;

namespace PawPal.Infrastructure.Common;

/// <summary>
/// Global exception handler for unhandled exceptions.
/// Keeps the same ErrorDto format as the previous middleware.
/// </summary>
public sealed class MarketExceptionHandler(
    ILogger<MarketExceptionHandler> logger,
    IHostEnvironment env
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex, CancellationToken ct)
    {
        // If the response has already started, let it bubble up
        if (ctx.Response.HasStarted)
        {
            logger.LogWarning(ex, "Response already started, letting the exception bubble.");
            return false;
        }

        var traceId = Activity.Current?.Id ?? ctx.TraceIdentifier;

        logger.LogError(ex,
            "Unhandled exception. Path: {Path}, Method: {Method}, TraceId: {TraceId}, User: {User}",
            ctx.Request.Path,
            ctx.Request.Method,
            traceId,
            ctx.User.Identity?.Name ?? "anonymous");


        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = ex switch
        {
            PawPalNotFoundException => StatusCodes.Status404NotFound,
            PawPalConflictException or MarketBusinessRuleException => StatusCodes.Status409Conflict,
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var error = BuildErrorDto(ex, env.IsDevelopment(), traceId);

        await ctx.Response.WriteAsJsonAsync(error, cancellationToken: ct);
        return true; // prevents rethrowing the exception
    }

    private static ErrorDto BuildErrorDto(Exception ex, bool isDev, string traceId)
    {
        string code = "internal.error";
        string message = "An error occurred. Please try again.";

        switch (ex)
        {
            case PawPalNotFoundException:
            case PawPalConflictException:
            case MarketBusinessRuleException:
                code = "entity.error";
                message = ex.Message;
                break;

            case ValidationException vex:
                code = "validation.error";
                message = "Validation failed: " +
                          string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                break;
        }

        return new ErrorDto
        {
            Code = code,
            Message = message,
            TraceId = traceId,
            Details = isDev ? ex.ToString() : null // stack trace only in Development environment
        };
    }
}
