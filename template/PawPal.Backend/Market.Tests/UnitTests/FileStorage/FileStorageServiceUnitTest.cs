using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using PawPal.Application.Common.Exceptions;
using PawPal.Infrastructure.Common;
using System.Text;
using Xunit;

namespace PawPal.Tests.UnitTests.FileStorage
{
    public class FileStorageServiceUnitTest : IDisposable
    {
        private readonly string _webRoot;
        private readonly FileStorageService _sut;

        public FileStorageServiceUnitTest()
        {
            _webRoot = Path.Combine(Path.GetTempPath(), "PawPalFileStorageTests_" + Guid.NewGuid());
            Directory.CreateDirectory(_webRoot);

            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(x => x.WebRootPath).Returns(_webRoot);

            _sut = new FileStorageService(envMock.Object);
        }

        public void Dispose()
        {
            if (Directory.Exists(_webRoot))
            {
                Directory.Delete(_webRoot, recursive: true);
            }
        }

        private static IFormFile MakeFile(string fileName, int sizeBytes = 10)
        {
            var content = Encoding.UTF8.GetBytes(new string('a', sizeBytes));
            var stream = new MemoryStream(content);
            return new FormFile(stream, 0, content.Length, "file", fileName);
        }

        [Fact]
        public async Task SaveFileAsync_ShouldThrow_WhenExtensionNotAllowed()
        {
            var file = MakeFile("malicious.exe");

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _sut.SaveFileAsync(file, "posts/Post_1", CancellationToken.None));
        }

        [Fact]
        public async Task SaveFileAsync_ShouldThrow_WhenFileExceedsSizeLimit()
        {
            var file = MakeFile("big.jpg", sizeBytes: 11 * 1024 * 1024);

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _sut.SaveFileAsync(file, "posts/Post_1", CancellationToken.None));
        }

        [Fact]
        public async Task SaveFileAsync_ShouldNotUseOriginalFileName()
        {
            var file = MakeFile("../../evil.jpg");

            var relativePath = await _sut.SaveFileAsync(file, "posts/Post_1", CancellationToken.None);

            Assert.DoesNotContain("evil", relativePath);
            Assert.DoesNotContain("..", relativePath);
            Assert.EndsWith(".jpg", relativePath);
        }

        [Fact]
        public async Task SaveFileAsync_ShouldGenerateDifferentNames_ForFilesWithSameOriginalName()
        {
            var file1 = MakeFile("photo.jpg");
            var file2 = MakeFile("photo.jpg");

            var path1 = await _sut.SaveFileAsync(file1, "posts/Post_1", CancellationToken.None);
            var path2 = await _sut.SaveFileAsync(file2, "posts/Post_1", CancellationToken.None);

            Assert.NotEqual(path1, path2);
        }

        [Fact]
        public async Task SaveFileAsync_ThenReadFileAsync_ShouldRoundTripContent()
        {
            var content = "hello world";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var file = new FormFile(stream, 0, stream.Length, "file", "photo.png");

            var relativePath = await _sut.SaveFileAsync(file, "posts/Post_1", CancellationToken.None);
            var readBack = await _sut.ReadFileAsync(relativePath, CancellationToken.None);

            Assert.Equal(content, Encoding.UTF8.GetString(readBack));
        }

        [Fact]
        public async Task SaveFilesAsync_ShouldSaveEveryFile()
        {
            var files = new List<IFormFile> { MakeFile("a.jpg"), MakeFile("b.png"), MakeFile("c.webp") };

            var savedPaths = await _sut.SaveFilesAsync(files, "posts/Post_1", CancellationToken.None);

            Assert.Equal(3, savedPaths.Count);
            Assert.Equal(3, savedPaths.Distinct().Count());
        }

        [Fact]
        public async Task DeleteFolder_ShouldRemoveEverythingSavedUnderIt()
        {
            var file = MakeFile("photo.jpg");
            var relativePath = await _sut.SaveFileAsync(file, "posts/Post_99", CancellationToken.None);

            _sut.DeleteFolder("posts/Post_99");

            // The whole containing directory is gone, so this can surface as either exception
            // depending on platform.
            await Assert.ThrowsAnyAsync<IOException>(
                () => _sut.ReadFileAsync(relativePath, CancellationToken.None));
        }

        [Fact]
        public void DeleteFolder_ShouldNotThrow_WhenFolderDoesNotExist()
        {
            _sut.DeleteFolder("posts/Post_does_not_exist");
        }
    }
}
