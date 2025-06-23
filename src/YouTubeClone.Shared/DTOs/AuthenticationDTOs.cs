using System.ComponentModel.DataAnnotations;

namespace YouTubeClone.Shared.DTOs;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public bool RememberMe { get; set; }
}

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    [Required]
    public string UserName { get; set; } = string.Empty;
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
}

public class AuthenticationResponse
{
    public bool IsSuccess { get; set; }
    
    public string? Token { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
    
    public UserDto? User { get; set; }
    
    public string? ErrorMessage { get; set; }
}

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty;
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? ProfilePictureUrl { get; set; }
    
    public string? ChannelName { get; set; }
    
    public string? ChannelDescription { get; set; }
    
    public int SubscriberCount { get; set; }
    
    public bool IsVerified { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
