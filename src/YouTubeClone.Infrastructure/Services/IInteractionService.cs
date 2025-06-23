using YouTubeClone.Shared.DTOs;

namespace YouTubeClone.Infrastructure.Services;

public interface IInteractionService
{
    Task<bool> AddCommentAsync(string userId, CreateCommentRequest request);
    Task<bool> UpdateCommentAsync(string userId, string commentId, UpdateCommentRequest request);
    Task<bool> DeleteCommentAsync(string userId, string commentId);
    Task<bool> ToggleLikeAsync(string userId, LikeRequest request);
    Task<bool> ToggleSubscriptionAsync(string userId, SubscriptionRequest request);
    Task<List<CommentDto>> GetVideoCommentsAsync(string videoId);
}
