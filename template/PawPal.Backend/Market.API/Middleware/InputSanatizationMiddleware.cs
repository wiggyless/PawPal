
using System.Text;
using System.Text.RegularExpressions;

public class InputSanitizationMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly string[] InspectableContentTypes =
    {
        "application/json",
        "text/json",
        "text/plain",
        "application/x-www-form-urlencoded",
    };

  
    private const long MaxInspectableBodyBytes = 1 * 1024 * 1024; 

    public InputSanitizationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        foreach (var param in context.Request.Query)
        {
            if (ContainsXssPatterns(param.Value.ToString()))
            {
                await RejectAsync(context);
                return;
            }
        }

        if (await BodyContainsXssPatternsAsync(context.Request))
        {
            await RejectAsync(context);
            return;
        }

        await _next(context);
    }

    private static async Task<bool> BodyContainsXssPatternsAsync(HttpRequest request)
    {
        if (!IsInspectable(request))
        {
            return false;
        }

        request.EnableBuffering();

        using var reader = new StreamReader(
            request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 4096,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        return ContainsXssPatterns(body);
    }

    private static bool IsInspectable(HttpRequest request)
    {
        if (request.ContentLength is null or 0)
        {
            return false;
        }

        if (request.ContentLength > MaxInspectableBodyBytes)
        {
            return false;
        }

        var contentType = request.ContentType;
        if (string.IsNullOrEmpty(contentType))
        {
            return false;
        }

        return InspectableContentTypes.Any(t =>
            contentType.StartsWith(t, StringComparison.OrdinalIgnoreCase));
    }

    private static async Task RejectAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = "Invalid input detected" });
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
