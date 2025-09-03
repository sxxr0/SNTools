using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SNTools.Game;
using SNTools.UI.Models;
using System.Collections.Specialized;
using System.Windows.Media;

namespace SNTools.UI.Pages.ViewModels;

public partial class LobbyPlayersPageViewModel : ObservableObject
{
    [ObservableProperty]
    private LobbyPlayerButtonModel? _selectedPlayer;

    public LobbyPlayerButtonModel[] LobbyPlayerButtonModels { get; } =
    [
        new(1, Brushes.Red),
        new(2, Brushes.Green),
        new(3, Brushes.Blue),
        new(4, Brushes.Yellow),
        new(5, Brushes.Purple),
        new(6, Brushes.Orange)
    ];

    internal void Load()
    {
        UpdatePlayerList();
        LobbyPlayerManager.Players.CollectionChanged += OnPlayerListChanged;
    }

    private void OnPlayerListChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => UpdatePlayerList();

    private void UpdatePlayerList()
    {
        foreach (var model in LobbyPlayerButtonModels)
        {
            model.LobbyPlayer = LobbyPlayerManager.Players.FirstOrDefault(p => p.LobbySlot == model.Slot);
        }
    }

    internal void Unload()
    {
        LobbyPlayerManager.Players.CollectionChanged -= OnPlayerListChanged;
    }

    [RelayCommand]
    private void MakeMasterClient()
    {
        SelectedPlayer?.LobbyPlayer?.SetMasterClient();
    }
}