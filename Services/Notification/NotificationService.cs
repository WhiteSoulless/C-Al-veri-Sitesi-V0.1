namespace SalesApp.Services.Notification;

public class NotificationService : INotificationService
{
    public Task ShowAlertAsync(string title, string message, string cancelText = "Tamam")
    {
        if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0].Page != null)
        {
            return Application.Current.Windows[0].Page!.DisplayAlert(title, message, cancelText);
        }
        return Task.CompletedTask;
    }

    public Task<bool> ShowConfirmAsync(string title, string message, string acceptText = "Evet", string cancelText = "İptal")
    {
        if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0].Page != null)
        {
            return Application.Current.Windows[0].Page!.DisplayAlert(title, message, acceptText, cancelText);
        }
        return Task.FromResult(false);
    }
}
