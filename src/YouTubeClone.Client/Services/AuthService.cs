using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text.Json;
using YouTubeClone.Shared.DTOs;

namespace YouTubeClone.Client.Services;

public interface IAuthService
{
    Task<AuthenticationResponse> LoginAsync(LoginRequest request);
    Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task<UserDto?> GetCurrentUserAsync();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";
    private const string UserKey = "currentUser";

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

            if (result?.IsSuccess == true && !string.IsNullOrEmpty(result.Token))
            {
                await _localStorage.SetItemAsync(TokenKey, result.Token);
                await _localStorage.SetItemAsync(UserKey, result.User);
            }

            return result ?? new AuthenticationResponse { IsSuccess = false, ErrorMessage = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            return new AuthenticationResponse { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

            if (result?.IsSuccess == true && !string.IsNullOrEmpty(result.Token))
            {
                await _localStorage.SetItemAsync(TokenKey, result.Token);
                await _localStorage.SetItemAsync(UserKey, result.User);
            }

            return result ?? new AuthenticationResponse { IsSuccess = false, ErrorMessage = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            return new AuthenticationResponse { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(UserKey);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(TokenKey);
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        return await _localStorage.GetItemAsync<UserDto>(UserKey);
    }
}
