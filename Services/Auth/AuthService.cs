using System.Diagnostics;
using SalesApp.Models.Auth;

namespace SalesApp.Services.Auth;

public class AuthService : IAuthService
{
    private User? _currentUser;
    private readonly Dictionary<string, OtpRequest> _pendingOtps = new();

    public User? CurrentUser => _currentUser;
    public bool IsAuthenticated => _currentUser != null;

    public async Task<bool> LoginAsync(string emailOrPhone, string password)
    {
        await Task.Delay(500); // Gerçekçi ağ simülasyonu

        if (string.IsNullOrWhiteSpace(emailOrPhone) || string.IsNullOrWhiteSpace(password))
            return false;

        _currentUser = new User
        {
            Id = 1,
            FullName = "Can Kaya",
            Email = emailOrPhone.Contains('@') ? emailOrPhone : "can@example.com",
            PhoneNumber = emailOrPhone.Contains('@') ? "+905551234567" : emailOrPhone,
            IsPhoneVerified = true
        };

        return true;
    }

    public async Task<string> SendSmsOtpAsync(string phoneNumber)
    {
        await Task.Delay(300);

        string generatedCode = new Random().Next(100000, 999999).ToString();

        _pendingOtps[phoneNumber] = new OtpRequest
        {
            PhoneNumber = phoneNumber,
            Code = generatedCode,
            SentAt = DateTime.UtcNow,
            ExpireSeconds = 120
        };

        Debug.WriteLine($"[SMS SERVISI] Telefon: {phoneNumber} | 6 Haneli Doğrulama Kodunuz: {generatedCode}");

        return generatedCode;
    }

    public async Task<bool> VerifySmsOtpAsync(string phoneNumber, string code)
    {
        await Task.Delay(300);

        if (_pendingOtps.TryGetValue(phoneNumber, out var otpRequest))
        {
            if (!otpRequest.IsExpired && otpRequest.Code == code)
            {
                if (_currentUser != null)
                    _currentUser.IsPhoneVerified = true;

                _pendingOtps.Remove(phoneNumber);
                return true;
            }
        }

        return false;
    }

    public async Task<bool> RegisterAsync(User user, string password)
    {
        await Task.Delay(500);
        _currentUser = user;
        return true;
    }

    public Task LogoutAsync()
    {
        _currentUser = null;
        _pendingOtps.Clear();
        return Task.CompletedTask;
    }
}
