using SalesApp.ViewModels.Auth;

namespace SalesApp.Views.Auth;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnGuestClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }
}
