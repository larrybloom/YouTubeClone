namespace YouTubeClone.Shared.Models;

public class Subscription
{
    public string Id { get; set; } = string.Empty;
    
    public string SubscriberId { get; set; } = string.Empty;
    
    public virtual User Subscriber { get; set; } = null!;
    
    public string CreatorId { get; set; } = string.Empty;
    
    public virtual User Creator { get; set; } = null!;
    
    public DateTime SubscribedAt { get; set; }
    
    public bool NotificationsEnabled { get; set; }
}
