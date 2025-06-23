using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace YouTubeClone.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobStorageService(string connectionString, string containerName = "videos")
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerName = containerName;
    }

    public async Task<string> UploadVideoAsync(Stream videoStream, string fileName, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(fileName);
        
        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType
        };

        await blobClient.UploadAsync(videoStream, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        });

        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadThumbnailAsync(Stream thumbnailStream, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("thumbnails");
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(fileName);
        
        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = "image/jpeg"
        };

        await blobClient.UploadAsync(thumbnailStream, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        });

        return blobClient.Uri.ToString();
    }

    public async Task<bool> DeleteVideoAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        
        var response = await blobClient.DeleteIfExistsAsync();
        return response.Value;
    }

    public async Task<bool> DeleteThumbnailAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("thumbnails");
        var blobClient = containerClient.GetBlobClient(fileName);
        
        var response = await blobClient.DeleteIfExistsAsync();
        return response.Value;
    }

    public async Task<Stream?> DownloadVideoAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        if (await blobClient.ExistsAsync())
        {
            var response = await blobClient.DownloadStreamingAsync();
            return response.Value.Content;
        }

        return null;
    }

    public string GetVideoUrl(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        return blobClient.Uri.ToString();
    }

    public string GetThumbnailUrl(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("thumbnails");
        var blobClient = containerClient.GetBlobClient(fileName);
        return blobClient.Uri.ToString();
    }
}
