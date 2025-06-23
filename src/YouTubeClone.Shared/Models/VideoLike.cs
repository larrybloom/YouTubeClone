namespace YouTubeClone.Shared.Models;

public class VideoLike
{
    public string Id { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public virtual User User { get; set; } = null!;
    
    public string VideoId { get; set; } = string.Empty;
    
    public virtual Video Video { get; set; } = null!;
    
    public bool IsLike { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
