using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesApp.Services.Auth;
using SalesApp.Services.Notification;

namespace SalesApp.ViewModels.Auth;

[QueryProperty(nameof(PhoneNumber), "phone")]
[QueryProperty(nameof(TestCode), "testcode")]
public partial class OtpViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly INotificationService _notificationService;

    [ObservableProperty]
    private string _phoneNumber = string.Empty;

    [ObservableProperty]
    private string _testCode = string.Empty;

    [ObservableProperty]
    private string _code = string.Empty;

    [ObservableProperty]
    private int _countdownSeconds = 120;

    [ObservableProperty]
    private bool _canResend;

    public OtpViewModel(
        IAuthService authService,
        INotificationService notificationService)
    {
        _authService = authService;
        _notificationService = notificationService;

        Title = "SMS Doğrulama";
    }

    [RelayCommand]
    public async Task VerifyCodeAsync()
    {
        if (string.IsNullOrWhiteSpace(Code) || Code.Length < 6)
        {
            await _notificationService.ShowAlertAsync("Eksik Kod", "Lütfen 6 haneli SMS kodunu eksiksiz giriniz.");
            return;
        }

        IsBusy = true;
        try
        {
            bool isSuccess = await _authService.VerifySmsOtpAsync(PhoneNumber, Code);
            if (isSuccess)
            {
                await _notificationService.ShowAlertAsync("Harika!", "Telefon numaranız başarıyla doğrulandı. Mağazaya yönlendiriliyorsunuz.");
                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                await _notificationService.ShowAlertAsync("Hatalı Kod", "Girdiğiniz SMS kodu hatalı veya süresi dolmuş.");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task ResendCodeAsync()
    {
        IsBusy = true;
        try
        {
            string newCode = await _authService.SendSmsOtpAsync(PhoneNumber);
            TestCode = newCode;
            CountdownSeconds = 120;
            CanResend = false;

            await _notificationService.ShowAlertAsync("Yeni Kod Gönderildi", $"Yeni SMS Kodunuz: {newCode}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
