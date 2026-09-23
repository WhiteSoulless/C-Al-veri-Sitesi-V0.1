using SalesApp.Models.Auth;

namespace SalesApp.Services.Auth;

public interface IAuthService
{
    User? CurrentUser { get; }
    bool IsAuthenticated { get; }

    Task<bool> LoginAsync(string emailOrPhone, string password);
    Task<string> SendSmsOtpAsync(string phoneNumber);
    Task<bool> VerifySmsOtpAsync(string phoneNumber, string code);
    Task<bool> RegisterAsync(User user, string password);
    Task LogoutAsync();
}
