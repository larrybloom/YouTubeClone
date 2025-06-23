using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeClone.Shared.DTOs;

namespace YouTubeClone.Infrastructure.Services;

public class YouTubeApiService : IYouTubeApiService
{
    private readonly YouTubeService _youtubeService;
    private readonly string _apiKey;

    public YouTubeApiService(string apiKey)
    {
        _apiKey = apiKey;
        _youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
            ApplicationName = "YouTubeClone"
        });
    }

    public async Task<YouTubeSearchResponse> SearchVideosAsync(YouTubeSearchRequest request)
    {
        var searchRequest = _youtubeService.Search.List("snippet");
        searchRequest.Q = request.Query;
        searchRequest.Type = request.Type;
        searchRequest.Order = GetOrderEnum(request.Order);
        searchRequest.MaxResults = request.MaxResults;
        searchRequest.PublishedAfter = request.PublishedAfter;
        searchRequest.PublishedBefore = request.PublishedBefore;
        searchRequest.ChannelId = request.ChannelId;
        searchRequest.RegionCode = request.RegionCode;
        searchRequest.RelevanceLanguage = request.RelevanceLanguage;

        var searchResponse = await searchRequest.ExecuteAsync();
        var videoIds = searchResponse.Items.Select(item => item.Id.VideoId).ToList();

        // Get detailed video information
        var videosRequest = _youtubeService.Videos.List("snippet,contentDetails,statistics");
        videosRequest.Id = string.Join(",", videoIds);
        var videosResponse = await videosRequest.ExecuteAsync();

        var videos = videosResponse.Items.Select(video => new YouTubeVideoDto
        {
            Id = video.Id,
            Title = video.Snippet.Title,
            Description = video.Snippet.Description,
            ThumbnailUrl = video.Snippet.Thumbnails.High.Url,
            ChannelTitle = video.Snippet.ChannelTitle,
            ChannelId = video.Snippet.ChannelId,
            PublishedAt = video.Snippet.PublishedAt ?? DateTime.MinValue,
            Duration = video.ContentDetails.Duration,
            ViewCount = (long)(video.Statistics.ViewCount ?? 0),
            LikeCount = (long)(video.Statistics.LikeCount ?? 0),
            Tags = video.Snippet.Tags?.ToArray() ?? Array.Empty<string>(),
            CategoryId = video.Snippet.CategoryId,
            DefaultLanguage = video.Snippet.DefaultLanguage ?? "en"
        }).ToList();

        return new YouTubeSearchResponse
        {
            Videos = videos,
            NextPageToken = searchResponse.NextPageToken,
            PrevPageToken = searchResponse.PrevPageToken,
            TotalResults = searchResponse.PageInfo.TotalResults ?? 0,
            ResultsPerPage = searchResponse.PageInfo.ResultsPerPage ?? 0
        };
    }

    public async Task<List<YouTubeVideoDto>> GetTrendingVideosAsync(YouTubeTrendingRequest request)
    {
        var videosRequest = _youtubeService.Videos.List("snippet,contentDetails,statistics");
        videosRequest.Chart = VideosResource.ListRequest.ChartEnum.MostPopular;
        videosRequest.RegionCode = request.RegionCode;
        videosRequest.VideoCategoryId = request.CategoryId;
        videosRequest.MaxResults = request.MaxResults;

        var response = await videosRequest.ExecuteAsync();

        return response.Items.Select(video => new YouTubeVideoDto
        {
            Id = video.Id,
            Title = video.Snippet.Title,
            Description = video.Snippet.Description,
            ThumbnailUrl = video.Snippet.Thumbnails.High.Url,
            ChannelTitle = video.Snippet.ChannelTitle,
            ChannelId = video.Snippet.ChannelId,
            PublishedAt = video.Snippet.PublishedAt ?? DateTime.MinValue,
            Duration = video.ContentDetails.Duration,
            ViewCount = (long)(video.Statistics.ViewCount ?? 0),
            LikeCount = (long)(video.Statistics.LikeCount ?? 0),
            Tags = video.Snippet.Tags?.ToArray() ?? Array.Empty<string>(),
            CategoryId = video.Snippet.CategoryId,
            DefaultLanguage = video.Snippet.DefaultLanguage ?? "en"
        }).ToList();
    }

    public async Task<YouTubeChannelDto?> GetChannelAsync(string channelId)
    {
        var channelsRequest = _youtubeService.Channels.List("snippet,statistics");
        channelsRequest.Id = channelId;

        var response = await channelsRequest.ExecuteAsync();
        var channel = response.Items.FirstOrDefault();

        if (channel == null)
            return null;

        return new YouTubeChannelDto
        {
            Id = channel.Id,
            Title = channel.Snippet.Title,
            Description = channel.Snippet.Description,
            ThumbnailUrl = channel.Snippet.Thumbnails.High.Url,
            SubscriberCount = (long)(channel.Statistics.SubscriberCount ?? 0),
            VideoCount = (long)(channel.Statistics.VideoCount ?? 0),
            ViewCount = (long)(channel.Statistics.ViewCount ?? 0),
            PublishedAt = channel.Snippet.PublishedAt ?? DateTime.MinValue,
            Country = channel.Snippet.Country ?? string.Empty
        };
    }

    private SearchResource.ListRequest.OrderEnum? GetOrderEnum(string? order)
    {
        return order?.ToLower() switch
        {
            "date" => SearchResource.ListRequest.OrderEnum.Date,
            "rating" => SearchResource.ListRequest.OrderEnum.Rating,
            "relevance" => SearchResource.ListRequest.OrderEnum.Relevance,
            "title" => SearchResource.ListRequest.OrderEnum.Title,
            "videocount" => SearchResource.ListRequest.OrderEnum.VideoCount,
            "viewcount" => SearchResource.ListRequest.OrderEnum.ViewCount,
            _ => SearchResource.ListRequest.OrderEnum.Relevance
        };
    }
}
