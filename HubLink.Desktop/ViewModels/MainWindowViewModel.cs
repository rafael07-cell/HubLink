using CommunityToolkit.Mvvm.ComponentModel;
using HubLink.Application.Configuration;
using System;

namespace HubLink.Desktop.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public string Endpoint { get; }
    public string Branch { get; }
    public string Status => "HubLink shell is running. Configuration loaded.";

    public MainWindowViewModel(HubLinkSettings settings)
    {
        Branch = settings.BranchCode;
        try
        {
            Endpoint = settings.BuildShipmentsUri().ToString();
        }
        catch (InvalidOperationException ex)
        {
            Endpoint = "Invalid configuration: " + ex.Message;
        }
    }
}
