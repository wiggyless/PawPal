using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;

namespace PawPal.Infrastructure.Common;

public sealed class FileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    // The one standard applied to every upload endpoint (post images, user avatars, news
    // photos) — see IFileStorageService.
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB per file

    private string WebRoot => env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

    public async Task<string> SaveFileAsync(IFormFile file, string subFolder, CancellationToken cancellationToken)
    {
        ValidateFile(file);

        var normalizedSubFolder = subFolder.Replace('\\', '/').Trim('/');
        var directory = Path.Combine(WebRoot, normalizedSubFolder.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(directory);

        // Never trust the client-supplied file name — only its (already validated) extension
        // survives, and the file is stored under a freshly generated name.
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var generatedFileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(directory, generatedFileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return $"/{normalizedSubFolder}/{generatedFileName}";
    }

    public async Task<IReadOnlyList<string>> SaveFilesAsync(IEnumerable<IFormFile> files, string subFolder, CancellationToken cancellationToken)
    {
        var savedPaths = new List<string>();
        foreach (var file in files)
        {
            savedPaths.Add(await SaveFileAsync(file, subFolder, cancellationToken));
        }
        return savedPaths;
    }

    public void DeleteFolder(string subFolder)
    {
        var normalizedSubFolder = subFolder.Replace('\\', '/').Trim('/');
        var directory = Path.Combine(WebRoot, normalizedSubFolder.Replace('/', Path.DirectorySeparatorChar));
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    public async Task<byte[]> ReadFileAsync(string relativePath, CancellationToken cancellationToken)
    {
        var normalizedRelativePath = relativePath.Replace('\\', '/').TrimStart('/');
        var fullPath = Path.Combine(WebRoot, normalizedRelativePath.Replace('/', Path.DirectorySeparatorChar));
        return await File.ReadAllBytesAsync(fullPath, cancellationToken);
    }

    private static void ValidateFile(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            throw new PawPalConflictException("A file is required.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
        {
            throw new PawPalConflictException($"File type '{extension}' is not allowed.");
        }

        if (file.Length > MaxFileSizeBytes)
        {
            throw new PawPalConflictException($"File '{file.FileName}' exceeds the {MaxFileSizeBytes / 1024 / 1024}MB limit.");
        }
    }
}
