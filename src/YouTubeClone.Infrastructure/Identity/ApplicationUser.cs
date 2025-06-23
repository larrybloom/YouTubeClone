using Microsoft.AspNetCore.Identity;
using YouTubeClone.Shared.Models;

namespace YouTubeClone.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
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
