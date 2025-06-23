namespace YouTubeClone.Shared.Models;

public class CommentLike
{
    public string Id { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public virtual User User { get; set; } = null!;
    
    public string CommentId { get; set; } = string.Empty;
    
    public virtual Comment Comment { get; set; } = null!;
    
    public bool IsLike { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
