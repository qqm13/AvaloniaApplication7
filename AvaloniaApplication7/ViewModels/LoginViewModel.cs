using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication7.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting;
    
    public LoginViewModel()
    {
    }

    [RelayCommand]
    public void Test()
    {
        
    }
  
}