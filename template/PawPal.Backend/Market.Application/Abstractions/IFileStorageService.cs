using Microsoft.AspNetCore.Http;

namespace PawPal.Application.Abstractions;

/// <summary>
/// Handles saving, reading, and removing uploaded files (post images, user avatars, news
/// photos) on behalf of Application-layer handlers. Every save validates file type and size
/// against the same standard policy and stores the file under a generated name, so callers
/// never trust a client-supplied file name.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Validates <paramref name="file"/> (extension + size), saves it under
    /// wwwroot/<paramref name="subFolder"/> using a newly generated file name, and returns the
    /// web-relative path (e.g. "/posts/Post_5/3fa1...c2.jpg") to store alongside the entity.
    /// Throws <see cref="PawPalConflictException"/> if the file fails validation.
    /// </summary>
    Task<string> SaveFileAsync(IFormFile file, string subFolder, CancellationToken cancellationToken);

    /// <summary>Validates and saves each file under the same <paramref name="subFolder"/>.</summary>
    Task<IReadOnlyList<string>> SaveFilesAsync(IEnumerable<IFormFile> files, string subFolder, CancellationToken cancellationToken);

    /// <summary>Deletes every file previously saved under <paramref name="subFolder"/>, if any.</summary>
    void DeleteFolder(string subFolder);

    /// <summary>Reads the raw bytes of a file previously saved at <paramref name="relativePath"/> (a value returned by SaveFileAsync/SaveFilesAsync).</summary>
    Task<byte[]> ReadFileAsync(string relativePath, CancellationToken cancellationToken);
}
