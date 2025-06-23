using System.ComponentModel.DataAnnotations;

namespace YouTubeClone.Shared.Models;

public class Video
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public string VideoUrl { get; set; } = string.Empty;
    
    public string ThumbnailUrl { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public virtual User User { get; set; } = null!;
    
    public DateTime UploadDate { get; set; }
    
    public DateTime? PublishDate { get; set; }
    
    public VideoStatus Status { get; set; } = VideoStatus.Processing;
    
    public VideoVisibility Visibility { get; set; } = VideoVisibility.Private;
    
    public long ViewCount { get; set; }
    
    public int LikeCount { get; set; }
    
    public int DislikeCount { get; set; }
    
    public string? Duration { get; set; }
    
    public string? Category { get; set; }
    
    public string[] Tags { get; set; } = Array.Empty<string>();
    
    // Video quality options
    public Dictionary<string, string> QualityUrls { get; set; } = new();
    
    // Navigation properties
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<VideoLike> Likes { get; set; } = new List<VideoLike>();
    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}

public enum VideoStatus
{
    Processing,
    Ready,
    Failed
}

public enum VideoVisibility
{
    Private,
    Unlisted,
    Public
}
