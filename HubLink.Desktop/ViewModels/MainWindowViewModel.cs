using CommunityToolkit.Mvvm.ComponentModel;

namespace HubLink.Desktop.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public string Status => "HubLink shell is running.";
}
