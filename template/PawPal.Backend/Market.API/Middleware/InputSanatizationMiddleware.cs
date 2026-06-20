
using System.Text.RegularExpressions;

public class InputSanitizationMiddleware
{
    private readonly RequestDelegate _next;

    public InputSanitizationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        foreach (var param in context.Request.Query)
        {
            if (ContainsXssPatterns(param.Value.ToString()))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid input detected" });
                return;
            }
        }

        await _next(context);
    }

    private static bool ContainsXssPatterns(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        var patterns = new[]
        {
            @"<script[^>]*>", @"javascript:", @"on\w+\s*=",
            @"<iframe", @"<object", @"<embed", @"eval\s*\("
        };

        return patterns.Any(p =>
            Regex.IsMatch(input, p, RegexOptions.IgnoreCase));
    }
}