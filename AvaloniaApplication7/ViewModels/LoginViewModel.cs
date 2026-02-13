using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaApplication7.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Path = Avalonia.Controls.Shapes.Path;


namespace AvaloniaApplication7.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly ApiService _apiService;
    private readonly IStorageProvider _storageProvider;
    
    [ObservableProperty]
    private string _username;
    //private string _greeting;
    [ObservableProperty]
    private string _password;
    [ObservableProperty]
    private string _rememberSession;
    [ObservableProperty]
    private string _errorMessage;
    [ObservableProperty]
    private string _isLoading;
    
    public LoginViewModel(IStorageProvider storageProvider)
    {
        _apiService = new ApiService();
        _storageProvider = storageProvider;
        Task.Run(LoadSavedTokenAsync);
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            IsLoading = true.ToString();
            ErrorMessage = string.Empty;
            var credintial = new CredentialDTO
            {
                Username = _username,
                PasswordHash = _password
            };
            var loginResult = await _apiService.LoginAsync(credintial);
            if (loginResult?.Token != null)
            {
                if (!string.IsNullOrEmpty(RememberSession))
                {
                    await SaveTokenAsync(loginResult.Token);
                }
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorMessage = "Неверное имя пользователя или пароль";
            }
            else
            {
                ErrorMessage = $"Ошибка сети: {ex.Message}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Произошла ошибка: {ex.Message}";
        }
        finally
        {
            IsLoading = false.ToString();
        }
        
        }

    private async Task SaveTokenAsync(string token)
    {
        try
        {
           if(_storageProvider == null) return;
           var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AvaloniaApp");
           Directory.CreateDirectory(appDataPath); 
           var tokenPath = Path.Combine(appDataPath, "session_token.data");
           var tolenData = new TokenData
           {
               Token = token,
               SavedAt = DateTime.Now,
           };
           var json = JsonSerializer.Serialize(tolenData);
           await File.WriteAllTextAsync(tokenPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error save token {ex.Message}");
        }

    }

    private async Task LoadSavedTokenAsync()
    {
        try
        {
            var file = await _storageProvider.TryGetWellKnownFolderAsync(WellKnownFolder.ApplicationData);
            if (file != null)
            {
                var tokenFile = await file.GetFileAsync("session_token.dat");
                if (tokenFile != null)
                {
                    await using var stream = await tokenFile.OpenReadAsync();
                    using var reader = new System.IO.StreamReader(stream);
                    var token = await reader.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        RememberSession = true.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading token {ex.Message}");
        }

        
    }
    }

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient();
        {
            _httpClient.BaseAddress = new Uri("https://localhost:5001");
        }
        

    }

    public async Task<LoginResult> LoginAsync(CredentialDTO credential)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", credential);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LoginResult>();
    }
}

public class LoginResult
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
