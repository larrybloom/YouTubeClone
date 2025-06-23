namespace YouTubeClone.Infrastructure.Services;

public interface IVideoProcessingService
{
    Task<VideoProcessingResult> ProcessVideoAsync(Stream videoStream, string fileName);
}

public class VideoProcessingResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan Duration { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string? ThumbnailUrl { get; set; }
    public Dictionary<string, string> Qualities { get; set; } = new();
}
