using System.ComponentModel.DataAnnotations;
using YouTubeClone.Shared.Models;

namespace YouTubeClone.Shared.DTOs;

public class VideoUploadRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public VideoVisibility Visibility { get; set; } = VideoVisibility.Private;
    
    public string? Category { get; set; }
    
    public string[]? Tags { get; set; }
}

public class VideoUploadResponse
{
    public bool IsSuccess { get; set; }
    
    public string? VideoId { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public string? UploadUrl { get; set; }
}

public class VideoDto
{
    public string Id { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string VideoUrl { get; set; } = string.Empty;
    
    public string ThumbnailUrl { get; set; } = string.Empty;
    
    public UserDto User { get; set; } = null!;
    
    public DateTime UploadDate { get; set; }
    
    public DateTime? PublishDate { get; set; }
    
    public VideoStatus Status { get; set; }
    
    public VideoVisibility Visibility { get; set; }
    
    public long ViewCount { get; set; }
    
    public int LikeCount { get; set; }
    
    public int DislikeCount { get; set; }
    
    public string? Duration { get; set; }
    
    public string? Category { get; set; }
    
    public string[] Tags { get; set; } = Array.Empty<string>();
    
    public Dictionary<string, string> QualityUrls { get; set; } = new();
}

public class VideoSearchRequest
{
    public string? Query { get; set; }
    
    public string? Category { get; set; }
    
    public string[]? Tags { get; set; }
    
    public DateTime? FromDate { get; set; }
    
    public DateTime? ToDate { get; set; }
    
    public string? SortBy { get; set; }
    
    public bool SortDescending { get; set; } = true;
    
    public int Page { get; set; } = 1;
    
    public int PageSize { get; set; } = 20;
}

public class VideoSearchResponse
{
    public List<VideoDto> Videos { get; set; } = new();
    
    public int TotalCount { get; set; }
    
    public int CurrentPage { get; set; }
    
    public int TotalPages { get; set; }
}

public class VideoUpdateRequest
{
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public VideoVisibility Visibility { get; set; }
    
    public string? Category { get; set; }
    
    public string[]? Tags { get; set; }
    
    public string? ThumbnailUrl { get; set; }
}
