using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouTubeClone.Infrastructure.Services;
using YouTubeClone.Shared.DTOs;

namespace YouTubeClone.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly IVideoProcessingService _videoProcessingService;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IYouTubeApiService _youTubeApiService;
    private readonly IInteractionService _interactionService;

    public VideoController(
        IVideoProcessingService videoProcessingService,
        IBlobStorageService blobStorageService,
        IYouTubeApiService youTubeApiService,
        IInteractionService interactionService)
    {
        _videoProcessingService = videoProcessingService;
        _blobStorageService = blobStorageService;
        _youTubeApiService = youTubeApiService;
        _interactionService = interactionService;
    }

    [HttpPost("upload")]
    [Authorize]
    [RequestSizeLimit(1024 * 1024 * 1024)] // 1GB
    public async Task<ActionResult<VideoUploadResponse>> UploadVideo([FromForm] IFormFile file, [FromForm] VideoUploadRequest request)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new VideoUploadResponse { IsSuccess = false, ErrorMessage = "No file uploaded" });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

        try
        {
            using var stream = file.OpenReadStream();
            var result = await _videoProcessingService.ProcessVideoAsync(stream, fileName);

            if (!result.IsSuccess)
                return BadRequest(new VideoUploadResponse { IsSuccess = false, ErrorMessage = result.ErrorMessage });

            return new VideoUploadResponse
            {
                IsSuccess = true,
                VideoId = fileName,
                UploadUrl = result.Qualities.FirstOrDefault().Value
            };
        }
        catch (Exception ex)
        {
            return StatusCode(500, new VideoUploadResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Error processing video: {ex.Message}"
            });
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<YouTubeSearchResponse>> SearchVideos([FromQuery] YouTubeSearchRequest request)
    {
        try
        {
            var result = await _youTubeApiService.SearchVideosAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error searching videos: {ex.Message}" });
        }
    }

    [HttpGet("trending")]
    public async Task<ActionResult<List<YouTubeVideoDto>>> GetTrendingVideos([FromQuery] YouTubeTrendingRequest request)
    {
        try
        {
            var result = await _youTubeApiService.GetTrendingVideosAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error getting trending videos: {ex.Message}" });
        }
    }

    [HttpGet("{videoId}/comments")]
    public async Task<ActionResult<List<CommentDto>>> GetVideoComments(string videoId)
    {
        try
        {
            var comments = await _interactionService.GetVideoCommentsAsync(videoId);
            return Ok(comments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error getting comments: {ex.Message}" });
        }
    }

    [HttpPost("{videoId}/comments")]
    [Authorize]
    public async Task<ActionResult<bool>> AddComment(string videoId, [FromBody] CreateCommentRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        request.VideoId = videoId;

        try
        {
            var result = await _interactionService.AddCommentAsync(userId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error adding comment: {ex.Message}" });
        }
    }

    [HttpPost("{videoId}/like")]
    [Authorize]
    public async Task<ActionResult<bool>> ToggleLike(string videoId, [FromBody] bool isLike)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var request = new LikeRequest
        {
            ItemId = videoId,
            Type = LikeType.Video,
            IsLike = isLike
        };

        try
        {
            var result = await _interactionService.ToggleLikeAsync(userId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error toggling like: {ex.Message}" });
        }
    }
}
