using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication7.ViewModels;

namespace AvaloniaApplication7.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        var storageProvider = this.StorageProvider;
        DataContext = new LoginViewModel(storageProvider);
    }
}