using System.Net.Http.Json;
using YouTubeClone.Shared.DTOs;

namespace YouTubeClone.Client.Services;

public interface IVideoService
{
    Task<YouTubeSearchResponse> SearchVideosAsync(YouTubeSearchRequest request);
    Task<List<YouTubeVideoDto>> GetTrendingVideosAsync(YouTubeTrendingRequest request);
    Task<VideoUploadResponse> UploadVideoAsync(MultipartFormDataContent content);
    Task<List<CommentDto>> GetVideoCommentsAsync(string videoId);
    Task<bool> AddCommentAsync(string videoId, CreateCommentRequest request);
    Task<bool> ToggleVideoLikeAsync(string videoId, bool isLike);
}

public class VideoService : IVideoService
{
    private readonly HttpClient _httpClient;

    public VideoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<YouTubeSearchResponse> SearchVideosAsync(YouTubeSearchRequest request)
    {
        try
        {
            var queryString = BuildQueryString(request);
            var response = await _httpClient.GetFromJsonAsync<YouTubeSearchResponse>($"api/video/search{queryString}");
            return response ?? new YouTubeSearchResponse();
        }
        catch (Exception)
        {
            return new YouTubeSearchResponse();
        }
    }

    public async Task<List<YouTubeVideoDto>> GetTrendingVideosAsync(YouTubeTrendingRequest request)
    {
        try
        {
            var queryString = BuildQueryString(request);
            var response = await _httpClient.GetFromJsonAsync<List<YouTubeVideoDto>>($"api/video/trending{queryString}");
            return response ?? new List<YouTubeVideoDto>();
        }
        catch (Exception)
        {
            return new List<YouTubeVideoDto>();
        }
    }

    public async Task<VideoUploadResponse> UploadVideoAsync(MultipartFormDataContent content)
    {
        try
        {
            var response = await _httpClient.PostAsync("api/video/upload", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<VideoUploadResponse>();
                return result ?? new VideoUploadResponse { IsSuccess = false, ErrorMessage = "Failed to parse response" };
            }

            return new VideoUploadResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Upload failed with status code: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new VideoUploadResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Upload failed: {ex.Message}"
            };
        }
    }

    public async Task<List<CommentDto>> GetVideoCommentsAsync(string videoId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<CommentDto>>($"api/video/{videoId}/comments");
            return response ?? new List<CommentDto>();
        }
        catch (Exception)
        {
            return new List<CommentDto>();
        }
    }

    public async Task<bool> AddCommentAsync(string videoId, CreateCommentRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/video/{videoId}/comments", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ToggleVideoLikeAsync(string videoId, bool isLike)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/video/{videoId}/like", isLike);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static string BuildQueryString<T>(T request) where T : class
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.GetValue(request) != null)
            .Select(p => $"{p.Name.ToLower()}={Uri.EscapeDataString(p.GetValue(request)?.ToString() ?? "")}");

        var queryString = string.Join("&", properties);
        return string.IsNullOrEmpty(queryString) ? "" : $"?{queryString}";
    }
}
