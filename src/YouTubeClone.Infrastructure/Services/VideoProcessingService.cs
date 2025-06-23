using System.Diagnostics;
using YouTubeClone.Shared.Models;

namespace YouTubeClone.Infrastructure.Services;

public class VideoProcessingService : IVideoProcessingService
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly string _ffmpegPath;
    private readonly string _tempPath;

    public VideoProcessingService(
        IBlobStorageService blobStorageService,
        string ffmpegPath = "ffmpeg",
        string? tempPath = null)
    {
        _blobStorageService = blobStorageService;
        _ffmpegPath = ffmpegPath;
        _tempPath = tempPath ?? Path.GetTempPath();
    }

    public async Task<VideoProcessingResult> ProcessVideoAsync(Stream videoStream, string fileName)
    {
        var tempInputPath = Path.Combine(_tempPath, $"input_{fileName}");
        var result = new VideoProcessingResult { Qualities = new Dictionary<string, string>() };

        try
        {
            // Save uploaded file to temp
            using (var fileStream = File.Create(tempInputPath))
            {
                await videoStream.CopyToAsync(fileStream);
            }

            // Get video info
            var videoInfo = await GetVideoInfoAsync(tempInputPath);
            result.Duration = videoInfo.Duration;
            result.Width = videoInfo.Width;
            result.Height = videoInfo.Height;

            // Generate thumbnail
            var thumbnailPath = await GenerateThumbnailAsync(tempInputPath);
            using (var thumbnailStream = File.OpenRead(thumbnailPath))
            {
                result.ThumbnailUrl = await _blobStorageService.UploadThumbnailAsync(
                    thumbnailStream,
                    $"{Path.GetFileNameWithoutExtension(fileName)}_thumb.jpg"
                );
            }

            // Process different qualities
            var qualities = new[] { "1080p", "720p", "480p", "360p" };
            foreach (var quality in qualities)
            {
                if (CanProcessQuality(videoInfo, quality))
                {
                    var processedPath = await TranscodeVideoAsync(tempInputPath, quality);
                    using (var processedStream = File.OpenRead(processedPath))
                    {
                        var qualityFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{quality}.mp4";
                        var url = await _blobStorageService.UploadVideoAsync(
                            processedStream,
                            qualityFileName,
                            "video/mp4"
                        );
                        result.Qualities.Add(quality, url);
                    }
                    File.Delete(processedPath);
                }
            }

            result.IsSuccess = true;
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.ErrorMessage = ex.Message;
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempInputPath))
                File.Delete(tempInputPath);
        }

        return result;
    }

    private async Task<VideoInfo> GetVideoInfoAsync(string inputPath)
    {
        var info = new VideoInfo();
        var args = $"-v quiet -print_format json -show_format -show_streams \"{inputPath}\"";
        
        var startInfo = new ProcessStartInfo
        {
            FileName = _ffmpegPath,
            Arguments = args,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        var output = await process!.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        // Parse ffprobe output
        // This is simplified - you'd want to properly parse the JSON output
        info.Duration = TimeSpan.FromSeconds(60); // Example
        info.Width = 1920;
        info.Height = 1080;

        return info;
    }

    private async Task<string> GenerateThumbnailAsync(string inputPath)
    {
        var outputPath = Path.Combine(_tempPath, $"thumb_{Path.GetFileName(inputPath)}.jpg");
        var args = $"-i \"{inputPath}\" -ss 00:00:01.000 -vframes 1 \"{outputPath}\"";

        var startInfo = new ProcessStartInfo
        {
            FileName = _ffmpegPath,
            Arguments = args,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        await process!.WaitForExitAsync();

        return outputPath;
    }

    private async Task<string> TranscodeVideoAsync(string inputPath, string quality)
    {
        var outputPath = Path.Combine(_tempPath, $"{Path.GetFileNameWithoutExtension(inputPath)}_{quality}.mp4");
        var (width, height) = GetDimensionsForQuality(quality);
        
        var args = $"-i \"{inputPath}\" -c:v libx264 -preset medium -crf 23 " +
                  $"-vf scale={width}:{height} -c:a aac -b:a 128k \"{outputPath}\"";

        var startInfo = new ProcessStartInfo
        {
            FileName = _ffmpegPath,
            Arguments = args,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        await process!.WaitForExitAsync();

        return outputPath;
    }

    private bool CanProcessQuality(VideoInfo originalVideo, string quality)
    {
        var (width, _) = GetDimensionsForQuality(quality);
        return originalVideo.Width >= width;
    }

    private (int width, int height) GetDimensionsForQuality(string quality)
    {
        return quality switch
        {
            "1080p" => (1920, 1080),
            "720p" => (1280, 720),
            "480p" => (854, 480),
            "360p" => (640, 360),
            _ => throw new ArgumentException($"Unsupported quality: {quality}")
        };
    }

    private class VideoInfo
    {
        public TimeSpan Duration { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
