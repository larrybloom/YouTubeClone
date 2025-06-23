using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouTubeClone.Infrastructure.Services;
using YouTubeClone.Shared.DTOs;

namespace YouTubeClone.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IInteractionService _interactionService;

    public UserController(IInteractionService interactionService)
    {
        _interactionService = interactionService;
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult<bool>> ToggleSubscription([FromBody] SubscriptionRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            var result = await _interactionService.ToggleSubscriptionAsync(userId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error toggling subscription: {ex.Message}" });
        }
    }

    [HttpPost("comments/{commentId}/like")]
    public async Task<ActionResult<bool>> ToggleCommentLike(string commentId, [FromBody] bool isLike)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var request = new LikeRequest
        {
            ItemId = commentId,
            Type = LikeType.Comment,
            IsLike = isLike
        };

        try
        {
            var result = await _interactionService.ToggleLikeAsync(userId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error toggling comment like: {ex.Message}" });
        }
    }

    [HttpPut("comments/{commentId}")]
    public async Task<ActionResult<bool>> UpdateComment(string commentId, [FromBody] UpdateCommentRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            var result = await _interactionService.UpdateCommentAsync(userId, commentId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error updating comment: {ex.Message}" });
        }
    }

    [HttpDelete("comments/{commentId}")]
    public async Task<ActionResult<bool>> DeleteComment(string commentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            var result = await _interactionService.DeleteCommentAsync(userId, commentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error deleting comment: {ex.Message}" });
        }
    }
}
