using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SNTools.Game.Features.Identity;

namespace SNTools.UI.Pages.ViewModels;

public partial class IdentityPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _playerName = string.Empty;

    internal void Refresh()
    {
        PlayerName = NameChanger.Name;
    }

    partial void OnPlayerNameChanged(string value)
    {
        NameChanger.Name = value;
    }

    [RelayCommand]
    private void RestoreDefaultName()
    {
        NameChanger.RestoreDefaultName();
        PlayerName = NameChanger.Name;
    }
}