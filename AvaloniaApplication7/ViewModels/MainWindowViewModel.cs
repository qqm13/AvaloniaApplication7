using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication7.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting;

    public MainWindowViewModel()
    {
    }

    [RelayCommand]
    public void Test()
    {
    }
}