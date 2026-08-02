using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;

namespace Shared.Infrastructure.Storage;

public class CloudinaryFileStorageService : IFileStorageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryFileStorageService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken = default)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = folder,
            PublicId = Guid.NewGuid().ToString("N"),
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (result.Error is not null)
        {
            throw new ApiException($"Image upload failed: {result.Error.Message}", 502);
        }

        return result.SecureUrl.ToString();
    }
}
