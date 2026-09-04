using PawPal.Shared.Models;

namespace PawPal.API.Extensions;

/// <summary>Converts ASP.NET's IFormFile into the transport-neutral FileUpload the
/// Application layer accepts, keeping IFormFile confined to the API layer.</summary>
public static class FormFileExtensions
{
    public static FileUpload ToFileUpload(this IFormFile file) => new()
    {
        Content = file.OpenReadStream(),
        FileName = file.FileName,
        ContentType = file.ContentType,
        Length = file.Length
    };

    public static List<FileUpload> ToFileUploads(this IEnumerable<IFormFile> files) =>
        files.Select(ToFileUpload).ToList();
}
