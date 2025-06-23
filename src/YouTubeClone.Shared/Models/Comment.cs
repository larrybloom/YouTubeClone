using System.ComponentModel.DataAnnotations;

namespace YouTubeClone.Shared.Models;

public class Comment
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public virtual User User { get; set; } = null!;
    
    public string VideoId { get; set; } = string.Empty;
    
    public virtual Video Video { get; set; } = null!;
    
    public string? ParentCommentId { get; set; }
    
    public virtual Comment? ParentComment { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public int LikeCount { get; set; }
    
    public int DislikeCount { get; set; }
    
    public bool IsEdited { get; set; }
    
    public bool IsDeleted { get; set; }
    
    // Navigation properties
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    public virtual ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();
}
