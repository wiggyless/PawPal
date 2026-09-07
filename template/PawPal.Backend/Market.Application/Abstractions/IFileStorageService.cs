using PawPal.Shared.Models;

namespace PawPal.Application.Abstractions;

public interface IFileStorageService
{

    Task<string> SaveFileAsync(FileUpload file, string subFolder, CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> SaveFilesAsync(IEnumerable<FileUpload> files, string subFolder, CancellationToken cancellationToken);
    void DeleteFolder(string subFolder);

    Task<byte[]> ReadFileAsync(string relativePath, CancellationToken cancellationToken);
}
