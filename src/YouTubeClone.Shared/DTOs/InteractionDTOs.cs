using System.ComponentModel.DataAnnotations;

namespace YouTubeClone.Shared.DTOs;

public class CommentDto
{
    public string Id { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
    
    public UserDto User { get; set; } = null!;
    
    public string VideoId { get; set; } = string.Empty;
    
    public string? ParentCommentId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public int LikeCount { get; set; }
    
    public int DislikeCount { get; set; }
    
    public bool IsEdited { get; set; }
    
    public List<CommentDto> Replies { get; set; } = new();
}

public class CreateCommentRequest
{
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public string VideoId { get; set; } = string.Empty;
    
    public string? ParentCommentId { get; set; }
}

public class UpdateCommentRequest
{
    [Required]
    public string Content { get; set; } = string.Empty;
}

public class LikeRequest
{
    [Required]
    public string ItemId { get; set; } = string.Empty;
    
    [Required]
    public LikeType Type { get; set; }
    
    public bool IsLike { get; set; }
}

public enum LikeType
{
    Video,
    Comment
}

public class SubscriptionDto
{
    public string CreatorId { get; set; } = string.Empty;
    
    public UserDto Creator { get; set; } = null!;
    
    public DateTime SubscribedAt { get; set; }
    
    public bool NotificationsEnabled { get; set; }
}

public class SubscriptionRequest
{
    [Required]
    public string CreatorId { get; set; } = string.Empty;
    
    public bool NotificationsEnabled { get; set; }
}

public class PlaylistDto
{
    public string Id { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public UserDto User { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public string? ThumbnailUrl { get; set; }
    
    public List<VideoDto> Videos { get; set; } = new();
}

public class CreatePlaylistRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string[]? VideoIds { get; set; }
}

public class UpdatePlaylistRequest
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string[]? VideoIds { get; set; }
}
