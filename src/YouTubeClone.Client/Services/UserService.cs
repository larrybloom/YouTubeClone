using System.Net.Http.Json;
using YouTubeClone.Shared.DTOs;

namespace YouTubeClone.Client.Services;

public interface IUserService
{
    Task<bool> ToggleSubscriptionAsync(SubscriptionRequest request);
    Task<bool> ToggleCommentLikeAsync(string commentId, bool isLike);
    Task<bool> UpdateCommentAsync(string commentId, UpdateCommentRequest request);
    Task<bool> DeleteCommentAsync(string commentId);
}

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ToggleSubscriptionAsync(SubscriptionRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/subscribe", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ToggleCommentLikeAsync(string commentId, bool isLike)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/user/comments/{commentId}/like", isLike);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCommentAsync(string commentId, UpdateCommentRequest request)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/comments/{commentId}", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteCommentAsync(string commentId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/user/comments/{commentId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
