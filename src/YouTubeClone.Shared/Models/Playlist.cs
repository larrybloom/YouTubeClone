using System.ComponentModel.DataAnnotations;

namespace YouTubeClone.Shared.Models;

public class Playlist
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public virtual User User { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public PlaylistVisibility Visibility { get; set; } = PlaylistVisibility.Private;
    
    public string? ThumbnailUrl { get; set; }
    
    // Navigation properties
    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}

public enum PlaylistVisibility
{
    Private,
    Unlisted,
    Public
}
