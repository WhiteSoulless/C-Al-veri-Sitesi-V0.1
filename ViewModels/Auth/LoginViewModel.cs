using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesApp.Services.Auth;
using SalesApp.Services.Network;
using SalesApp.Services.Notification;

namespace SalesApp.ViewModels.Auth;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly IConnectivityService _connectivityService;
    private readonly INotificationService _notificationService;

    [ObservableProperty]
    private string _phoneNumber = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isSmsLoginMode = true; // SMS ile giriş veya şifre ile giriş

    public LoginViewModel(
        IAuthService authService,
        IConnectivityService connectivityService,
        INotificationService notificationService)
    {
        _authService = authService;
        _connectivityService = connectivityService;
        _notificationService = notificationService;

        Title = "Giriş Yap";
    }

    [RelayCommand]
    public async Task SendSmsCodeAsync()
    {
        if (string.IsNullOrWhiteSpace(PhoneNumber) || PhoneNumber.Length < 10)
        {
            await _notificationService.ShowAlertAsync("Uyarı", "Lütfen geçerli bir telefon numarası giriniz (Örn: 5551234567).");
            return;
        }

        if (!_connectivityService.IsConnected)
        {
            await _notificationService.ShowAlertAsync("Bağlantı Yok", "SMS kodu alabilmek için internet bağlantınız açık olmalıdır.");
            return;
        }

        IsBusy = true;
        try
        {
            string code = await _authService.SendSmsOtpAsync(PhoneNumber);
            await _notificationService.ShowAlertAsync(
                "SMS Doğrulama Kodu Gönderildi 📩",
                $"Telefonunuza 6 haneli kod gönderildi!\n(Test Kodu: {code})");

            // Doğrulama ekranına geçiş yapılabilir
            await Shell.Current.GoToAsync($"//OtpPage?phone={PhoneNumber}&testcode={code}");
        }
        catch (Exception ex)
        {
            await _notificationService.ShowAlertAsync("Hata", $"SMS gönderilemedi: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task LoginWithPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(Password))
        {
            await _notificationService.ShowAlertAsync("Eksik Bilgi", "Lütfen telefon/e-posta ve şifrenizi giriniz.");
            return;
        }

        if (!_connectivityService.IsConnected)
        {
            await _notificationService.ShowAlertAsync("Bağlantı Yok", "Giriş yapabilmek için lütfen internete bağlanın.");
            return;
        }

        IsBusy = true;
        try
        {
            bool success = await _authService.LoginAsync(PhoneNumber, Password);
            if (success)
            {
                await _notificationService.ShowAlertAsync("Başarılı", "Giriş yapıldı, hoş geldiniz!");
                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                await _notificationService.ShowAlertAsync("Giriş Başarısız", "Bilgileriniz hatalı, lütfen tekrar deneyin.");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
