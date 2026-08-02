namespace Shared.Core.Abstractions;

public interface IFileStorageService
{
    /// <summary>Uploads an image into the given folder (e.g. "products", "logo") and returns its public URL.</summary>
    Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken = default);
}
