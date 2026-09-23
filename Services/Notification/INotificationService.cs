namespace SalesApp.Services.Notification;

public interface INotificationService
{
    Task ShowAlertAsync(string title, string message, string cancelText = "Tamam");
    Task<bool> ShowConfirmAsync(string title, string message, string acceptText = "Evet", string cancelText = "İptal");
}
