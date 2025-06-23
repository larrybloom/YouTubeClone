namespace YouTubeClone.Shared.DTOs;

public class YouTubeVideoDto
{
    public string Id { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string ThumbnailUrl { get; set; } = string.Empty;
    
    public string ChannelTitle { get; set; } = string.Empty;
    
    public string ChannelId { get; set; } = string.Empty;
    
    public DateTime PublishedAt { get; set; }
    
    public string Duration { get; set; } = string.Empty;
    
    public long ViewCount { get; set; }
    
    public long LikeCount { get; set; }
    
    public string[] Tags { get; set; } = Array.Empty<string>();
    
    public string CategoryId { get; set; } = string.Empty;
    
    public string DefaultLanguage { get; set; } = string.Empty;
}

public class YouTubeChannelDto
{
    public string Id { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string ThumbnailUrl { get; set; } = string.Empty;
    
    public long SubscriberCount { get; set; }
    
    public long VideoCount { get; set; }
    
    public long ViewCount { get; set; }
    
    public DateTime PublishedAt { get; set; }
    
    public string Country { get; set; } = string.Empty;
}

public class YouTubeSearchRequest
{
    public string Query { get; set; } = string.Empty;
    
    public string? Type { get; set; } = "video";
    
    public string? Order { get; set; } = "relevance";
    
    public int MaxResults { get; set; } = 25;
    
    public string? PublishedAfter { get; set; }
    
    public string? PublishedBefore { get; set; }
    
    public string? ChannelId { get; set; }
    
    public string? RegionCode { get; set; }
    
    public string? RelevanceLanguage { get; set; }
}

public class YouTubeSearchResponse
{
    public List<YouTubeVideoDto> Videos { get; set; } = new();
    
    public string? NextPageToken { get; set; }
    
    public string? PrevPageToken { get; set; }
    
    public int TotalResults { get; set; }
    
    public int ResultsPerPage { get; set; }
}

public class YouTubeTrendingRequest
{
    public string RegionCode { get; set; } = "US";
    
    public string CategoryId { get; set; } = "0";
    
    public int MaxResults { get; set; } = 50;
}

public class YouTubePlaylistDto
{
    public string Id { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string ThumbnailUrl { get; set; } = string.Empty;
    
    public string ChannelTitle { get; set; } = string.Empty;
    
    public string ChannelId { get; set; } = string.Empty;
    
    public DateTime PublishedAt { get; set; }
    
    public int ItemCount { get; set; }
    
    public List<YouTubeVideoDto> Videos { get; set; } = new();
}
