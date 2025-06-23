namespace YouTubeClone.Infrastructure.Services;

public interface IBlobStorageService
{
    Task<string> UploadVideoAsync(Stream videoStream, string fileName, string contentType);
    Task<string> UploadThumbnailAsync(Stream thumbnailStream, string fileName);
    Task<bool> DeleteVideoAsync(string fileName);
    Task<bool> DeleteThumbnailAsync(string fileName);
    Task<Stream?> DownloadVideoAsync(string fileName);
    string GetVideoUrl(string fileName);
    string GetThumbnailUrl(string fileName);
}
