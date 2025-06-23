using System.ComponentModel.DataAnnotations;

namespace YouTubeClone.Shared.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string UserName { get; set; } = string.Empty;
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? ProfilePictureUrl { get; set; }
    
    public string? ChannelName { get; set; }
    
    public string? ChannelDescription { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public int SubscriberCount { get; set; }
    
    public bool IsVerified { get; set; }
    
    // Navigation properties
    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<VideoLike> VideoLikes { get; set; } = new List<VideoLike>();
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public virtual ICollection<Subscription> Subscribers { get; set; } = new List<Subscription>();
}
