using YouTubeClone.Shared.DTOs;

namespace YouTubeClone.Infrastructure.Services;

public interface IYouTubeApiService
{
    Task<YouTubeSearchResponse> SearchVideosAsync(YouTubeSearchRequest request);
    Task<List<YouTubeVideoDto>> GetTrendingVideosAsync(YouTubeTrendingRequest request);
    Task<YouTubeChannelDto?> GetChannelAsync(string channelId);
}
