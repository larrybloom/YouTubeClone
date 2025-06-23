using Microsoft.EntityFrameworkCore;
using YouTubeClone.Infrastructure.Data;
using YouTubeClone.Infrastructure.Identity;
using YouTubeClone.Shared.DTOs;
using YouTubeClone.Shared.Models;

namespace YouTubeClone.Infrastructure.Services;

public class InteractionService : IInteractionService
{
    private readonly ApplicationDbContext _context;

    public InteractionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddCommentAsync(string userId, CreateCommentRequest request)
    {
        var comment = new Comment
        {
            Id = Guid.NewGuid().ToString(),
            Content = request.Content,
            UserId = userId,
            VideoId = request.VideoId,
            ParentCommentId = request.ParentCommentId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCommentAsync(string userId, string commentId, UpdateCommentRequest request)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null || comment.UserId != userId)
            return false;

        comment.Content = request.Content;
        comment.UpdatedAt = DateTime.UtcNow;
        comment.IsEdited = true;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCommentAsync(string userId, string commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null || comment.UserId != userId)
            return false;

        comment.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleLikeAsync(string userId, LikeRequest request)
    {
        switch (request.Type)
        {
            case LikeType.Video:
                return await ToggleVideoLikeAsync(userId, request);
            case LikeType.Comment:
                return await ToggleCommentLikeAsync(userId, request);
            default:
                return false;
        }
    }

    public async Task<bool> ToggleSubscriptionAsync(string userId, SubscriptionRequest request)
    {
        var existingSubscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.SubscriberId == userId && s.CreatorId == request.CreatorId);

        if (existingSubscription != null)
        {
            _context.Subscriptions.Remove(existingSubscription);
        }
        else
        {
            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                SubscriberId = userId,
                CreatorId = request.CreatorId,
                NotificationsEnabled = request.NotificationsEnabled,
                SubscribedAt = DateTime.UtcNow
            };
            _context.Subscriptions.Add(subscription);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<CommentDto>> GetVideoCommentsAsync(string videoId)
    {
        var comments = await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Replies)
            .ThenInclude(r => r.User)
            .Where(c => c.VideoId == videoId && !c.IsDeleted && c.ParentCommentId == null)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return comments.Select(MapCommentToDto).ToList();
    }

    private async Task<bool> ToggleVideoLikeAsync(string userId, LikeRequest request)
    {
        var existingLike = await _context.VideoLikes
            .Include(l => l.User)
            .Include(l => l.Video)
            .FirstOrDefaultAsync(l => l.UserId == userId && l.VideoId == request.ItemId);

        if (existingLike != null)
        {
            if (existingLike.IsLike == request.IsLike)
            {
                _context.VideoLikes.Remove(existingLike);
            }
            else
            {
                existingLike.IsLike = request.IsLike;
            }
        }
        else
        {
            var videoLike = new VideoLike
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                VideoId = request.ItemId,
                IsLike = request.IsLike,
                CreatedAt = DateTime.UtcNow
            };
            _context.VideoLikes.Add(videoLike);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> ToggleCommentLikeAsync(string userId, LikeRequest request)
    {
        var existingLike = await _context.CommentLikes
            .Include(l => l.User)
            .Include(l => l.Comment)
            .FirstOrDefaultAsync(l => l.UserId == userId && l.CommentId == request.ItemId);

        if (existingLike != null)
        {
            if (existingLike.IsLike == request.IsLike)
            {
                _context.CommentLikes.Remove(existingLike);
            }
            else
            {
                existingLike.IsLike = request.IsLike;
            }
        }
        else
        {
            var commentLike = new CommentLike
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                CommentId = request.ItemId,
                IsLike = request.IsLike,
                CreatedAt = DateTime.UtcNow
            };
            _context.CommentLikes.Add(commentLike);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private CommentDto MapCommentToDto(Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            User = new UserDto
            {
                Id = comment.User.Id,
                UserName = comment.User.UserName,
                Email = comment.User.Email,
                ProfilePictureUrl = comment.User.ProfilePictureUrl,
                ChannelName = comment.User.ChannelName
            },
            VideoId = comment.VideoId,
            ParentCommentId = comment.ParentCommentId,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            LikeCount = comment.Likes.Count(l => l.IsLike),
            DislikeCount = comment.Likes.Count(l => !l.IsLike),
            IsEdited = comment.IsEdited,
            Replies = comment.Replies.Where(r => !r.IsDeleted)
                                   .Select(MapCommentToDto)
                                   .ToList()
        };
    }
}
