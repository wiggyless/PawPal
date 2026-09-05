namespace PawPal.Shared.Models;

/// <summary>Transport-agnostic stand-in for an uploaded file, built by the API layer from the
/// incoming request so Application/Domain never depend on IFormFile.</summary>
public sealed class FileUpload
{
    public required Stream Content { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required long Length { get; init; }
}
